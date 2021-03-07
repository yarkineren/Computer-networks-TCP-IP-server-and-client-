using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace server_proj408
{
    public partial class Form1 : Form
    {
        //public struct file_count  //for keeping the count for identical files sent by the same user 
        //{
        //    public string fname;
        //    public int count;
        //};

        Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        List<Socket> clientSockets = new List<Socket>();
        List<string> usernames = new List<string>();        //username list
        //List<string> listFiles = new List<string>();        //file list
        //List<file_count> file_count_list = new List<file_count>(); //"file_count" list

        bool terminating = false;
        bool listening = false;

        public Form1()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);
            InitializeComponent();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            listening = false;
            terminating = true;
            Environment.Exit(0);
        }


        private void button_browse_Click(object sender, EventArgs e) //selects a folder to store
        {
            FolderBrowserDialog folderDlg = new FolderBrowserDialog();
            folderDlg.ShowNewFolderButton = true;
            DialogResult result = folderDlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                textBox_path.Text = folderDlg.SelectedPath;
                Environment.SpecialFolder root = folderDlg.RootFolder;
            }
            button_listen.Enabled = true;
        }

        private void button_listen_Click(object sender, EventArgs e)
        {
            int serverPort;

            if (Int32.TryParse(textBox_port.Text, out serverPort)) //checks the port number
            {
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, serverPort);
                serverSocket.Bind(endPoint);
                serverSocket.Listen(3);

                listening = true;
                button_listen.Enabled = false;
                button_browse.Enabled = false;

                Thread acceptThread = new Thread(Accept); //calls Accept to accept clients
                acceptThread.Start();

                logs.AppendText("Started listening on port: " + serverPort + "\n");

            }
            else
            {
                logs.AppendText("Please check port number \n");
            }
        }


        private void Accept()
        {
            while (listening)
            {

                try
                {
                    Socket newClient = serverSocket.Accept(); //accepsts the client first 


                    //--------------------------------------------------------


                    Byte[] buffer = new Byte[64];
                    newClient.Receive(buffer);  //receive username
                    string username = Encoding.Default.GetString(buffer);
                    username = username.Substring(0, username.IndexOf("\0"));

                    bool isEmpty = !usernames.Any(); //if usernames list is empty, it is true

                    string temp = "already connected\n";  //message to sent to client if its username is not unique
                    Byte[] buffer2 = Encoding.Default.GetBytes(temp);
                    string temp2 = "bla bla";   //dummy message to send otherwise
                    Byte[] buffer3 = Encoding.Default.GetBytes(temp2);

                    if (isEmpty) //if list is initially empty, username will directly be added to the list
                    {
                        clientSockets.Add(newClient);
                        logs.AppendText("A client is connected.\n");
                        newClient.Send(buffer3); //sends dummy message
                        usernames.Add(username);
                        logs.AppendText("Client username: " + username + "\n");
                        Thread receiveThread = new Thread(() => Receive(newClient, username)); // calls Receive function to recieve files
                        receiveThread.Start();
                    }
                    else
                    {
                        int i = 0;
                        bool con = true;
                        while (i < usernames.Count() && con == true)
                        {
                            if (usernames[i] == username) //if username is already exists in usernames list
                            {
                                logs.AppendText("This client already exist\n");
                                newClient.Send(buffer2); //sends message to client
                                newClient.Close();      // and closes the socket
                                con = false;
                            }
                            else
                            {
                                i++;
                            }
                        }
                        if (con == true) //if username could not be found in the list
                        {
                            clientSockets.Add(newClient);
                            logs.AppendText("A client is connected.\n");
                            newClient.Send(buffer3);
                            usernames.Add(username);
                            logs.AppendText("Client username: " + username + "\n");
                            Thread receiveThread = new Thread(() => Receive(newClient, username));
                            receiveThread.Start();
                        }
                    }

                    //------------------------------------------
                    //Thread receive2Thread = new Thread(() => userreceive(newClient)); 
                    //receive2Thread.Start();


                }
                catch
                {
                    if (terminating)
                    {
                        listening = false;
                    }
                    else
                    {
                        logs.AppendText("The socket stopped working.\n");
                    }

                }
            }

        }

        private static Mutex mut = new Mutex();

        private void Receive(Socket thisClient, string username)
        {
            bool connected = true;
            //int count=0;
            while (connected && !terminating)
            {
                try
                {
                    var stream = textBox_path.Text; //stream is initially the folder path

                    string[] splitted2 = stream.Split('\\');
                    string foldername = splitted2[splitted2.Length - 1]; //take only the folder name to send to

                    string path_filename = ""; //it will be equal to the path until the foldername
                    foreach (string s in splitted2)
                    {
                        if (!s.Equals(foldername))
                        {
                            path_filename += s + "\\";
                        }
                    }

                    string destfolder = path_filename + foldername + "\\record.txt";

                    var bufferrequest = new byte[32];
                    thisClient.Receive(bufferrequest);
                    string requested = Encoding.Default.GetString(bufferrequest); //taking message from client which says what client want to be done
                    requested = requested.Substring(0, requested.IndexOf("\0"));

                    if (requested == "I request my files")    //displays client's own files and also public files of other clients, with size and upload time
                    {
                        try
                        { //finds the database(record.txt) and reads it

                            mut.WaitOne();
                            if (File.Exists(destfolder))
                            {
                                using (var fileStream = File.OpenRead(destfolder))
                                {
                                    using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true))
                                    {
                                        String line;
                                        while ((line = streamReader.ReadLine()) != null)
                                        {
                                            string[] splitted3 = line.Split(' ');
                                            if (splitted3[0] == username || splitted3[5] == "public")
                                            {

                                                string rfile;
                                                if (splitted3[0] != username)  //if the file does not belong to that user, but it is public, display it with owner name
                                                {
                                                    rfile = splitted3[0] + " " + splitted3[1] + " " + splitted3[2] + " " + splitted3[3] + " " + splitted3[4];
                                                }
                                                else
                                                {
                                                    rfile = splitted3[1] + " " + splitted3[2] + " " + splitted3[3] + " " + splitted3[4]; //take only the folder name, size, upload time(date) to send to client
                                                }

                                                Byte[] filesn = new Byte[64];
                                                filesn = Encoding.Default.GetBytes(rfile);
                                                thisClient.Send(filesn);
                                                Thread.Sleep(1000);
                                            }
                                        }
                                        // Process line
                                    }
                                }

                            }
                            logs.AppendText("File list is sent to" + username + " \n");
                            mut.ReleaseMutex();
                        }

                        catch
                        {
                            logs.AppendText("Cannot found any file \n");
                            mut.ReleaseMutex();
                        }
                    }
                    else if (requested == "Delete")
                    {
                        var filenamen = new byte[32];
                        thisClient.Receive(filenamen);
                        string filedel = Encoding.Default.GetString(filenamen); //taking file name from client
                        filedel = filedel.Substring(0, filedel.IndexOf("\0"));
                        string folder = path_filename + foldername + "\\" + username + "_" + filedel + ".txt";  //find the path of that file
                        try
                        {
                            // check if file exists with its full path    
                            mut.WaitOne();
                            if (File.Exists(folder))
                            {
                                // ıf file found, delete it    
                                File.Delete(folder);
                                logs.AppendText(filedel + ".txt is deleted by " + username + " \n");
                                string message = filedel + ".txt is deleted ";
                                Byte[] buffer = new Byte[32];
                                buffer = Encoding.Default.GetBytes(message);
                                thisClient.Send(buffer);
                                //string tempFile = Path.GetTempFileName();
                                string tempFile = path_filename + foldername + "\\temp.txt";  //make a temp(empty) file in the selected folder of the server
                                using (FileStream sw = File.Open(tempFile, FileMode.Append, FileAccess.Write))
                                {
                                    using (var fileStream = File.OpenRead(destfolder))    //open record.txt(database) and read it
                                    {
                                        using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true))
                                        {
                                            String line;
                                            while ((line = streamReader.ReadLine()) != null)
                                            {
                                                string[] splitted3 = line.Split(' ');

                                                if (splitted3[0] != username || (username == splitted3[0] && (filedel + ".txt") != splitted3[1]))  //writes informations of files other than the file which has deleted into temp.txt
                                                {
                                                    byte[] share = new UTF8Encoding(true).GetBytes(line + Environment.NewLine);
                                                    sw.Write(share, 0, share.Length);
                                                }
                                            }
                                        }
                                    }
                                }
                                File.Delete(destfolder);   //delete the record.txt
                                File.Move(tempFile, destfolder);   // convert temp.txt file to record.txt

                            }
                            else
                            {
                                logs.AppendText(filedel + ".txt is not deleted \n");
                                string message = filedel + ".txt is not deleted ";
                                Byte[] buffer = new Byte[32];
                                buffer = Encoding.Default.GetBytes(message);
                                thisClient.Send(buffer);
                                
                            }
                            mut.ReleaseMutex();


                        }
                        catch
                        {
                            logs.AppendText("file cannot be found. \n");
                            string message = "file cannot be found. ";
                            Byte[] buffer = new Byte[32];
                            buffer = Encoding.Default.GetBytes(message);
                            thisClient.Send(buffer);
                            mut.ReleaseMutex();
                        }

                    }
                    else if (requested == "download")  //sends the requested file of that client, or public files
                    {

                        var filename_download = new byte[32];
                        thisClient.Receive(filename_download);
                        string filedown = Encoding.Default.GetString(filename_download); //taking file name from client
                        filedown = filedown.Substring(0, filedown.IndexOf("\0"));
                        string folderD = "";
                        mut.WaitOne();
                        if (File.Exists(destfolder))
                        {
                            using (var fileStream = File.OpenRead(destfolder))
                            {
                                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true))   //reads record.txt(database)
                                {
                                    String line;
                                    while ((line = streamReader.ReadLine()) != null)
                                    {

                                        string[] splitted3 = line.Split(' ');
                                        if (splitted3[0] == username && splitted3[1] == (filedown + ".txt"))   //if the file belongs to that client
                                        {
                                            folderD = path_filename + foldername + "\\" + username + "_" + filedown + ".txt";
                                            break;
                                        }
                                        else if ((splitted3[0] + "_" + splitted3[1]) == (filedown + ".txt") && splitted3[5] == "public")
                                        {  //if the file belongs to another client, but it is public
                                            folderD = path_filename + foldername + "\\" + filedown + ".txt";
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        mut.ReleaseMutex();


                        try
                        {
                            using (var file = File.OpenRead(folderD)) //opening the file
                            {
                                thisClient.Send(Encoding.Default.GetBytes("download"));
                                byte[] filename = Encoding.Default.GetBytes(folderD); //converts file name(taken as a variable) to byte

                                int filenamesize = filename.Length;  //file name's size
                                var filensize = BitConverter.GetBytes(filenamesize); //converting to byte 
                                thisClient.Send(filensize); //sending to the client
                                //Thread.Sleep(1000);
                                thisClient.Send(filename); //sending file name to client
                                //Thread.Sleep(1000);
                                var fileSize = BitConverter.GetBytes((int)file.Length); //converting file's size 
                                thisClient.Send(fileSize); //sending to the client
                                //Thread.Sleep(1000);
                                var sendBuffer = new byte[2048];
                                var bytesLeftToTransmit = fileSize; //it is initially the whole file size, while sending buffers(sendBuffer) it will decrement.
                                while (BitConverter.ToInt32(bytesLeftToTransmit, 0) > 0)
                                {
                                    var dataToSend = file.Read(sendBuffer, 0, sendBuffer.Length); //read inside of the file(to sendBuffer)

                                    int i = BitConverter.ToInt32(bytesLeftToTransmit, 0);
                                    int sub = i - dataToSend;
                                    byte[] sum = BitConverter.GetBytes(sub);
                                    bytesLeftToTransmit = sum;

                                    //loop until the socket have sent everything in the buffer.
                                    var offset = 0;

                                    while (dataToSend > 0)
                                    {
                                        var bytesSent = thisClient.Send(sendBuffer, offset, dataToSend, SocketFlags.None);
                                        dataToSend -= bytesSent;
                                        offset += bytesSent;
                                    }
                                }
                                //Thread.Sleep(1000);
                                logs.AppendText(filedown + ".txt is sent to " + username + "\n");
                            }
                        }
                        catch
                        {
                            logs.AppendText("file cannot be found. \n");
                        }

                    }

                    else if (requested == "public")   //converts the requested file of that client into public
                    {
                        var filenamen = new byte[32];
                        thisClient.Receive(filenamen);
                        string filep = Encoding.Default.GetString(filenamen); //taking file name from client
                        filep = filep.Substring(0, filep.IndexOf("\0"));
                        mut.WaitOne();
                        string tempFile = path_filename + foldername + "\\temp.txt";  //make a temp(empty) file in the selected folder of server
                        bool done = false;
                        using (FileStream sw = File.Open(tempFile, FileMode.Append, FileAccess.Write))
                        {
                            using (var fileStream = File.OpenRead(destfolder))
                            {
                                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true))
                                {
                                    String line;
                                    while ((line = streamReader.ReadLine()) != null)
                                    {
                                        string[] splitted3 = line.Split(' ');
                                        if (splitted3[0] == username && splitted3[1] == (filep + ".txt") && splitted3[5] == "private")  //finds the requested file of that user, if it is private, converts it to public, and write to temp.txt
                                        {
                                            byte[] share = new UTF8Encoding(true).GetBytes((splitted3[0] + " " + splitted3[1] + " " + splitted3[2] + " " + splitted3[3] + " " + splitted3[4] + " " + "public") + Environment.NewLine);  //if parent is public copy is public too
                                            sw.Write(share, 0, share.Length);
                                            done = true;
                                        }
                                        else    //directly writes that line into temp.txt
                                        {
                                            byte[] share = new UTF8Encoding(true).GetBytes(line + Environment.NewLine);  //if parent is public copy is public too
                                            sw.Write(share, 0, share.Length);
                                        }
                                    }
                                    // Process line
                                }
                            }
                        }

                        File.Delete(destfolder); //delete the record.txt
                        File.Move(tempFile, destfolder); // convert temp.txt file to record.txt
                        mut.ReleaseMutex();
                        if (done)
                        {
                            logs.AppendText(filep + ".txt is made public by " + username + "\n");
                            string message = filep + ".txt is made public ";
                            Byte[] buffer = new Byte[32];
                            buffer = Encoding.Default.GetBytes(message);
                            thisClient.Send(buffer);
                        }
                        else
                        {
                            logs.AppendText(filep + ".txt is not made public \n");
                            string message = filep + ".txt is not made public ";
                            Byte[] buffer = new Byte[32];
                            buffer = Encoding.Default.GetBytes(message);
                            thisClient.Send(buffer);   
                        }

                    }
                    else if (requested == "copy")   //if the clients wants a copy of her/his file in the server
                    {
                        var filenamen = new byte[32];
                        thisClient.Receive(filenamen);
                        string filep = Encoding.Default.GetString(filenamen); //taking file name from client
                        filep = filep.Substring(0, filep.IndexOf("\0"));

                        //if (filep.IndexOf('_') != -1)  //checks "_", takes original name if there is "_"
                        //{
                        //    string[] spl = filep.Split('_');
                        //    filep = spl[0];
                        //}

                        filep = filep + ".txt";
                        string stream2 = path_filename + foldername + "\\" + username + "_" + filep;  //path of the file which is going to be copied
                        mut.WaitOne();
                        bool ispublic = false;
                        using (var fileStream = File.OpenRead(destfolder))
                        {
                            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true))
                            {
                                String line;
                                while ((line = streamReader.ReadLine()) != null)
                                {
                                    string[] splitted3 = line.Split(' ');
                                    if (splitted3[0] == username && splitted3[1] == filep && splitted3[5] == "public")   //checks the file of that client is public or not, if it is public copy of that file will be public too
                                    {
                                        ispublic = true;
                                    }

                                }
                            }
                        }
                        mut.ReleaseMutex();

                        bool isFound = false;
                        int count = 0;

                        //looks to the record.txt in order to add count to file name
                        mut.WaitOne();
                        if (File.Exists(destfolder))
                        {
                            using (var fileStream2 = File.OpenRead(destfolder))
                            {
                                using (var streamReader = new StreamReader(fileStream2, Encoding.UTF8, true))
                                {
                                    //String line2;
                                    //while ((line2 = streamReader.ReadLine()) != null)
                                    //{
                                    //    string[] splitted3 = line2.Split(' ');
                                    //    if (splitted3[0] == username && splitted3[1] == filep && splitted3[5] == "public")
                                    //    {
                                    //        ispublic = true;
                                    //    }

                                    //}

                                    if (filep.IndexOf('_') != -1)  //checks "_", takes original name if there is "_"
                                    {
                                        string[] spl = filep.Split('_');
                                        filep = spl[0] + ".txt";
                                    }

                                    String line;
                                    string[] splittedf = filep.Split('.');
                                    string ffname = splittedf[0];
                                    string type = splittedf[splittedf.Length - 1];
                                    while ((line = streamReader.ReadLine()) != null)
                                    {
                                        string[] splitted3 = line.Split(' ');
                                        string fnamewcount = splitted3[1];
                                        string[] splitted4 = fnamewcount.Split('.');
                                        string ff = splitted4[0];                   //keeps the file name without ".txt"
                                        string[] splitted5 = ff.Split('_');
                                        string finalf = splitted5[0];
                                        if (splitted3[0] == username && finalf == ffname)
                                        {
                                            isFound = true;
                                            count++;
                                        }
                                    }
                                    // Process line

                                    if (!isFound)
                                    {
                                        stream = path_filename + foldername + "\\" + username + "_" + filep;
                                    }
                                    else   //if the file is found add the count of that file
                                    {
                                        filep = ffname + "_0" + count.ToString() + "." + type;
                                        stream = path_filename + foldername + "\\" + username + "_" + filep;
                                        while (File.Exists(stream))
                                        {
                                            count++;
                                            filep = ffname + "_0" + count.ToString() + "." + type;
                                            stream = path_filename + foldername + "\\" + username + "_" + filep;
                                        }
                                    }
                                }
                            }

                        }
                        mut.ReleaseMutex();
                        string sourceFile = stream2;
                        string destinationFile = stream;
                        try
                        {
                            File.Copy(sourceFile, destinationFile, true);   //copy the wanted file
                            long filesize;
                            using (var file = File.OpenRead(stream2))
                            {
                                filesize = file.Length;
                            }

                            logs.AppendText(filep + " is copied by " + username + " \n");
                            string message = filep + " is copied ";
                            Byte[] buffer = new Byte[32];
                            buffer = Encoding.Default.GetBytes(message);
                            thisClient.Send(buffer);

                            try
                            {
                                DateTime time = DateTime.Now;
                                string format = "M/dd/yyyy HH:mm";   //date and time when the file copied
                                string t = time.ToString(format);
                                mut.WaitOne();
                                using (FileStream fs = File.Open(destfolder, FileMode.Append, FileAccess.Write))
                                {
                                    //Open the record file and write inside
                                    Byte[] user = new UTF8Encoding(true).GetBytes(username + " "); //username
                                    fs.Write(user, 0, user.Length);
                                    byte[] FileName = new UTF8Encoding(true).GetBytes(filep + " ");  //filename
                                    fs.Write(FileName, 0, FileName.Length);
                                    byte[] fsize = new UTF8Encoding(true).GetBytes(filesize + " ");  //file size
                                    fs.Write(fsize, 0, fsize.Length);
                                    byte[] uploadtime = new UTF8Encoding(true).GetBytes(t + " ");  //time and date when file copied
                                    fs.Write(uploadtime, 0, uploadtime.Length);
                                    if (ispublic)
                                    {
                                        byte[] share = new UTF8Encoding(true).GetBytes("public" + Environment.NewLine);  //if parent is public copy is public too
                                        fs.Write(share, 0, share.Length);
                                    }
                                    else
                                    {
                                        byte[] share = new UTF8Encoding(true).GetBytes("private" + Environment.NewLine);  //otherwise it is private
                                        fs.Write(share, 0, share.Length);
                                    }
                                }
                                mut.ReleaseMutex();

                            }
                            catch (Exception Ex)
                            {
                                logs.AppendText(Ex.ToString());
                                mut.ReleaseMutex();
                            }
                        }
                        catch
                        {
                            logs.AppendText("cannot copied \n");
                            string message = "cannot copied ";
                            Byte[] buffer = new Byte[32];
                            buffer = Encoding.Default.GetBytes(message);
                            thisClient.Send(buffer);
                        }
                    }

                    else if (requested == "dummy")   //if client wants to upload, send a file to server
                    {
                        //var stream = textBox_path.Text; //stream is initially the folder path

                        var buffer = new byte[4];
                        thisClient.Receive(buffer);
                        int filenamesize = BitConverter.ToInt32(buffer, 0); //taking file name size from client

                        var buffer_filename = new byte[filenamesize]; //creating a byte array using filenamesize 
                        thisClient.Receive(buffer_filename);
                        string filename = Encoding.Default.GetString(buffer_filename);  //taking file name

                        var buffer2 = new byte[4];
                        thisClient.Receive(buffer2);
                        int filesize = BitConverter.ToInt32(buffer2, 0);  //taking file size


                        var receiveBuffer = new byte[filesize]; //creating a rerceive buffer using filesize

                        var bytesLeftToReceive = filesize;

                        string[] splitted = filename.Split('\\');
                        string fName = splitted[splitted.Length - 1]; //take only the file name to be recieved

                        //string[] splitted2 = stream.Split('\\');
                        //string foldername = splitted2[splitted2.Length - 1]; //take only the folder name to send to

                        //string path_filename = ""; //it will be equal to the path until the foldername
                        //foreach (string s in splitted2)
                        //{
                        //    if (!s.Equals(foldername))
                        //    {
                        //        path_filename += s + "\\";
                        //    }
                        //}

                        stream = path_filename + foldername + "\\" + username + "_" + fName; //merge all

                        //stream = stream + "\\" + username + "_" + fName;

                        //---------------------------------------------------------
                        /* This part is for adding count at the end of the file name 
                         when files with same filename received by the same user  */
                        //checks from record.txt(database)
                        bool isFound = false;
                        int count = 0;
                        mut.WaitOne();
                        if (File.Exists(destfolder))
                        {
                            using (var fileStream = File.OpenRead(destfolder))
                            {
                                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true))
                                {
                                    String line;
                                    string[] splittedf = fName.Split('.');
                                    string ffname = splittedf[0];
                                    string type = splittedf[splittedf.Length - 1];
                                    while ((line = streamReader.ReadLine()) != null)
                                    {
                                        string[] splitted3 = line.Split(' ');
                                        string fnamewcount = splitted3[1];
                                        string[] splitted4 = fnamewcount.Split('.');
                                        string ff = splitted4[0];
                                        string[] splitted5 = ff.Split('_');
                                        string finalf = splitted5[0];
                                        if (splitted3[0] == username && finalf == ffname)
                                        {
                                            isFound = true;
                                            count++;
                                        }
                                    }
                                    // Process line

                                    if (!isFound)
                                    {
                                        stream = path_filename + foldername + "\\" + username + "_" + fName;
                                    }
                                    else
                                    {
                                        fName = ffname + "_0" + count.ToString() + "." + type;
                                        stream = path_filename + foldername + "\\" + username + "_" + fName;
                                        while(File.Exists(stream))
                                        {
                                            count++;
                                            fName = ffname + "_0" + count.ToString() + "." + type;
                                            stream = path_filename + foldername + "\\" + username + "_" + fName;
                                        }
                                    }
                                }
                            }

                        }
                        mut.ReleaseMutex();

                        //--------------------------------------------------------------

                        FileStream target_file = File.Open(stream, FileMode.Append);

                        while (bytesLeftToReceive > 0)
                        {
                            //receive
                            var bytesRead = thisClient.Receive(receiveBuffer);
                            if (bytesRead == 0)
                                throw new InvalidOperationException("Remote endpoint disconnected\n");

                            //if the socket is used for other things after the file transfer
                            //we need to make sure that we do not copy that data
                            //to the file
                            int bytesToCopy = Math.Min(bytesRead, bytesLeftToReceive);

                            // write to file

                            BinaryWriter bWrite = new BinaryWriter(target_file);
                            bWrite.Write(receiveBuffer, 0, bytesRead);
                            //update our tracker
                            bytesLeftToReceive -= bytesToCopy;
                        }
                        // listFiles.Add(stream);
                        target_file.Close();
                        logs.AppendText("File received by " + username + ": " + fName + "\n");


                        mut.WaitOne();
                        try
                        {
                            DateTime time = DateTime.Now;
                            string format = "M/dd/yyyy HH:mm";   //upload date and time
                            string t = time.ToString(format);

                            if (!File.Exists(destfolder))
                            {
                                // Create a new file     
                                using (FileStream fs = File.Create(destfolder))
                                {
                                    //Add username, filename, size, upload time(date) info to the record file
                                    Byte[] user = new UTF8Encoding(true).GetBytes(username + " "); //username
                                    fs.Write(user, 0, user.Length);
                                    byte[] FileName = new UTF8Encoding(true).GetBytes(fName + " ");  //filename
                                    fs.Write(FileName, 0, FileName.Length);
                                    byte[] fsize = new UTF8Encoding(true).GetBytes(filesize + " ");  //file size
                                    fs.Write(fsize, 0, fsize.Length);
                                    byte[] uploadtime = new UTF8Encoding(true).GetBytes(t + " ");  //upload time
                                    fs.Write(uploadtime, 0, uploadtime.Length);
                                    byte[] share = new UTF8Encoding(true).GetBytes("private" + Environment.NewLine);  //info about public or private
                                    fs.Write(share, 0, share.Length);
                                }
                            }
                            else //if file already exists.
                            {
                                using (FileStream fs = File.Open(destfolder, FileMode.Append, FileAccess.Write))
                                {
                                    //Open the record file and write inside
                                    Byte[] user = new UTF8Encoding(true).GetBytes(username + " "); //username
                                    fs.Write(user, 0, user.Length);
                                    byte[] FileName = new UTF8Encoding(true).GetBytes(fName + " ");  //filename
                                    fs.Write(FileName, 0, FileName.Length);
                                    byte[] fsize = new UTF8Encoding(true).GetBytes(filesize + " ");  //file size
                                    fs.Write(fsize, 0, fsize.Length);
                                    byte[] uploadtime = new UTF8Encoding(true).GetBytes(t + " ");  //upload time
                                    fs.Write(uploadtime, 0, uploadtime.Length);
                                    byte[] share = new UTF8Encoding(true).GetBytes("private" + Environment.NewLine);  //info about public or private
                                    fs.Write(share, 0, share.Length);
                                }
                            }
                            mut.ReleaseMutex();
                        }

                        catch (Exception Ex)
                        {
                            logs.AppendText(Ex.ToString());
                            mut.ReleaseMutex();
                        }

                    }
                }
                catch
                {
                    if (!terminating)
                    {
                        logs.AppendText("A client has disconnected\n");
                        usernames.Remove(username);
                    }
                    thisClient.Close();
                    clientSockets.Remove(thisClient);
                    usernames.Remove(username);
                    connected = false;
                }
            }
        }
    }
}
//string userdestfolder = path_filename + foldername + "\\" + username + "_record.txt";
//try
//{
//    DateTime time = DateTime.Now;
//    string format = "M/dd/yyyy HH:mm";
//    string t = time.ToString(format);

//    if (!File.Exists(userdestfolder))
//    {
//        // Create a new file     
//        using (FileStream fs = File.Create(userdestfolder))
//        {
//            //Add username and filename info to the record file    
//            Byte[] user = new UTF8Encoding(true).GetBytes(username + " "); //username
//            fs.Write(user, 0, user.Length);
//            byte[] FileName = new UTF8Encoding(true).GetBytes(fName + " ");  //filename
//            fs.Write(FileName, 0, FileName.Length);
//            byte[] fsize = new UTF8Encoding(true).GetBytes(filesize + " ");  //filename
//            fs.Write(fsize, 0, fsize.Length);
//            byte[] uploadtime = new UTF8Encoding(true).GetBytes(t + Environment.NewLine);  //filename
//            fs.Write(uploadtime, 0, uploadtime.Length);
//        }
//    }
//    else //if file already exists.
//    {
//        using (FileStream fs = File.Open(userdestfolder, FileMode.Append, FileAccess.Write))
//        {
//            //Open the record file and write inside
//            Byte[] user = new UTF8Encoding(true).GetBytes(username + " "); //username
//            fs.Write(user, 0, user.Length);
//            byte[] FileName = new UTF8Encoding(true).GetBytes(fName + " ");  //filename
//            fs.Write(FileName, 0, FileName.Length);
//            byte[] fsize = new UTF8Encoding(true).GetBytes(filesize + " ");  //filename
//            fs.Write(fsize, 0, fsize.Length);
//            byte[] uploadtime = new UTF8Encoding(true).GetBytes(t + Environment.NewLine);  //filename
//            fs.Write(uploadtime, 0, uploadtime.Length);
//        }
//    }


//}
//catch (Exception Ex)
//{
//    logs.AppendText(Ex.ToString());
//}

//bool isFound = false;
//int idx = file_count_list.Count() - 1;
//                        if (file_count_list.Any()) //if file_count list is not empty
//                        {
//                            for (int i = 0; i<file_count_list.Count(); i++)
//                            {
//                                if (file_count_list[i].fname == stream)
//                                {
//                                    isFound = true;
//                                    file_count temp = file_count_list[i];
//temp.count += 1;
//                                    file_count_list[i] = temp;
//                                    idx = i;
//                                }
//                            }
//                            if (!isFound)
//                            {
//                                file_count temp = new file_count();
//temp.count = 0;
//                                temp.fname = stream;
//                                file_count_list.Add(temp);
//                                idx++;
//                            }
//                        }
//                        else
//                        {
//                            file_count temp = new file_count();
//temp.count = 0;
//                            temp.fname = stream;
//                            file_count_list.Add(temp);
//                            idx++;
//                        }


//                        if (listFiles.Any() && listFiles.Contains(stream))
//                        {
//                            file_count temp2 = file_count_list[idx];
//count = temp2.count;
//                            string[] splitted3 = fName.Split('.');
//string ffname = splitted3[0];
//string type = splitted3[splitted3.Length - 1];
//fName = ffname + "_0" + count.ToString() + "." + type;
//                            stream = path_filename + foldername + "\\" + username + "_" + ffname + "_0" + count.ToString() + "." + type;
//                        }
