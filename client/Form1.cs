using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace client
{
    public partial class Form1 : Form
    {
        string name;
        bool terminating = false;
        bool connected = false;
        Socket clientSocket;
        private static string shortFileName = "";
        private static string fileName = ""; 
        public Form1()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);
            InitializeComponent();
        }
        private void Form1_FormClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            connected = false;
            terminating = true;
            Environment.Exit(0);
        }

        private void button_connect_Click(object sender, EventArgs e) 
        {
            terminating = false; // to connect after disconnect
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            string IP = textBox_ip.Text;
            int portNum;
            name = textBox_name.Text;
            
            string serverRespond = "";

            if (name != "") //name cannot be empty
            {
                if (Int32.TryParse(textBox_port.Text, out portNum)) // port must be a valid number
                {
                    if (textBox_ip.Text != "") // ip part cannot be empty
                    {
                        try
                        {
                            clientSocket.Connect(IP, portNum);
                            send_message(name); // send username to server and wait for respond
                            serverRespond = receiveOneMessage(); // get the respond 
                            if (serverRespond != "already connected\n") // if unique
                            {
                                button_send.Enabled = true;
                                logs.Enabled = true;
                                button_browse.Enabled = true;
                                button_connect.Enabled = false;
                                button_disconnect.Enabled = true;
                                
                                connected = true;
                                logs.AppendText("Connected to the server\n");

                                Thread receiveThread = new Thread(Receive); //in case server wants to send an info
                                receiveThread.Start();

                            }
                            else // if username is already used
                            {
                                logs.AppendText("Your username must be unique.\n");
                                clientSocket.Close();       
                                connected = false;
                                logs.ScrollToCaret();
                            }
                        }
                        catch
                        {
                            logs.AppendText("Could not connect to the server!\n");
                            logs.ScrollToCaret();
                        }
                    }
                    else
                    {
                        logs.AppendText("Check the IP\n");
                        logs.ScrollToCaret();
                    }


                }
                else
                {
                    logs.AppendText("Check the port\n");
                    logs.ScrollToCaret();
                }
            }
            else
            {
                textBox_name.Text = "";
                logs.AppendText("Incorrect Name\n");
                logs.ScrollToCaret();
            }

        }

        private string receiveOneMessage() // receives only one message (respond message for username's uniqueness)
        {
            Byte[] buffer = new Byte[10000000];
            clientSocket.Receive(buffer);
            string incomingMessage = Encoding.Default.GetString(buffer);
            incomingMessage = incomingMessage.Substring(0, incomingMessage.IndexOf("\0"));
            return incomingMessage;
        }

        private void Receive()
        {
            
            while (connected)
            {
                try
                {
 
                    Byte[] buffer = new Byte[64];
                    clientSocket.Receive(buffer);
                    string inmssg = Encoding.Default.GetString(buffer);
                    inmssg = inmssg.Substring(0, inmssg.IndexOf('\0'));
                    if (inmssg== "download")
                    {
                        var stream = textBox_path.Text; //stream is initially the folder path

                        var buffera = new byte[4];
                        clientSocket.Receive(buffera);
                        int filenamesize = BitConverter.ToInt32(buffera, 0); //taking file name size from client

                        var buffer_filename = new byte[filenamesize]; //creating a byte array using filenamesize 
                        clientSocket.Receive(buffer_filename);
                        string filename = Encoding.Default.GetString(buffer_filename);  //taking file name
                        

                        var buffer2 = new byte[4];
                        clientSocket.Receive(buffer2);
                        int filesize = BitConverter.ToInt32(buffer2, 0);  //taking file size


                        var receiveBuffer = new byte[filesize]; //creating a rerceive buffer using filesize

                        var bytesLeftToReceive = filesize;

                        string[] splitted = filename.Split('\\');
                        string fName = splitted[splitted.Length - 1]; //take only the file name to be recieved
                        
                        string[] split = fName.Split('_');
                        string name1 = split[0];

                        if (name1 == name && split[1].IndexOf('.') == -1)
                        {
                            fName = split[1] + ".txt";
                        }
                        else if (name1 == name)
                        {
                            fName = split[1];
                        }
                        else if (name1 != name && split[1].IndexOf('.') == -1)
                        {
                            fName = split[0] + "_" + split[1] + ".txt";
                        }
                        else if (name1 != name)
                        {
                            fName = split[0] + "_" + split[1];
                        }
                        
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

                        stream = path_filename + foldername + "\\" + fName; //merge all

                        //stream = stream + "\\" + username + "_" + fName;

                        string destfolder = path_filename  + name + ".txt";

                        bool isFound = false;
                        int count = 0;
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
                                        string fnamewcount = splitted3[0];
                                        string[] splitted4 = fnamewcount.Split('.');
                                        string ff = splitted4[0];
                                        if (ff.IndexOf('0') != -1)
                                        {
                                            string[] splitted5 = ff.Split('_');
                                            ff = splitted5[splitted5.Length - 2];
                                            if (splitted5.Length > 2)
                                                ff = splitted5[splitted5.Length - 3] + "_" + splitted5[splitted5.Length - 2];
                                        }
                                        if ( ff == ffname)
                                        {
                                            isFound = true;
                                            count++;
                                        }
                                    }
                                    // Process line

                                    if (!isFound)
                                    {
                                        stream = path_filename + foldername + "\\" + fName;
                                    }
                                    else
                                    {
                                        fName = ffname + "_0" + count.ToString() + "." + type;
                                        stream = path_filename + foldername + "\\" + fName;
                                    }
                                }
                            }

                        }

                        FileStream target_file = File.Open(stream, FileMode.Append);

                        while (bytesLeftToReceive > 0)
                        {
                            //receive
                            var bytesRead = clientSocket.Receive(receiveBuffer);
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

                        target_file.Close();
                        logs.AppendText("File downloaded \n");

                        try
                        {
                            DateTime time = DateTime.Now;
                            string format = "M/dd/yyyy HH:mm";
                            string t = time.ToString(format);

                            if (!File.Exists(destfolder))
                            {
                                // Create a new file     
                                using (FileStream fs = File.Create(destfolder))
                                {
                                    //Add username and filename info to the record file
                                    //Byte[] user = new UTF8Encoding(true).GetBytes(name + " "); //username
                                    //fs.Write(user, 0, user.Length);
                                    byte[] FileName = new UTF8Encoding(true).GetBytes(fName + " ");  //filename
                                    fs.Write(FileName, 0, FileName.Length);
                                    byte[] fsize = new UTF8Encoding(true).GetBytes(filesize + " ");  //filename
                                    fs.Write(fsize, 0, fsize.Length);
                                    byte[] uploadtime = new UTF8Encoding(true).GetBytes(t + Environment.NewLine);  //filename
                                    fs.Write(uploadtime, 0, uploadtime.Length);
                                    //byte[] share = new UTF8Encoding(true).GetBytes("private" + Environment.NewLine);  //filename
                                    //fs.Write(share, 0, share.Length);
                                }
                            }
                            else //if file already exists.
                            {
                                using (FileStream fs = File.Open(destfolder, FileMode.Append, FileAccess.Write))
                                {
                                    //Open the record file and write inside
                                    //Byte[] user = new UTF8Encoding(true).GetBytes(name + " "); //username
                                    //fs.Write(user, 0, user.Length);
                                    byte[] FileName = new UTF8Encoding(true).GetBytes(fName + " ");  //filename
                                    fs.Write(FileName, 0, FileName.Length);
                                    byte[] fsize = new UTF8Encoding(true).GetBytes(filesize + " ");  //filename
                                    fs.Write(fsize, 0, fsize.Length);
                                    byte[] uploadtime = new UTF8Encoding(true).GetBytes(t  + Environment.NewLine);  //filename
                                    fs.Write(uploadtime, 0, uploadtime.Length);
                                    //byte[] share = new UTF8Encoding(true).GetBytes("private" + Environment.NewLine);  //filename
                                    //fs.Write(share, 0, share.Length);
                                }
                            }

                        }
                        catch (Exception Ex)
                        {
                            logs.AppendText(Ex.ToString());
                        }

                    }

                    else
                    {
                        logs.AppendText("Server:    " + inmssg + "\n");
                        //Thread.Sleep(2000);
                    }
                }
                catch           //if server is disconnected 
                {
                    if(!terminating && button_disconnect.Enabled == true)
                    {
                        logs.AppendText("The server has disconnected\n");
                        button_connect.Enabled = true;
                        txtFile.Enabled= false;
                        button_send.Enabled = false;
             
                    }
                    clientSocket.Close();
                    connected = false;
                }
               
            }
        }

        private void send_message(string message) //sends username
        {
            Byte[] buffer = new Byte[32];
            buffer = Encoding.Default.GetBytes(message);
            clientSocket.Send(buffer);
        }

        private void button_browse_Click(object sender, EventArgs e) //selects a txt file to send
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = "File Sharing Client";
            dlg.ShowDialog();
            txtFile.Text = dlg.FileName;
            fileName = dlg.FileName;
            shortFileName = dlg.SafeFileName; 
        }

        private void button_send_Click(object sender, EventArgs e)  //calls SendFile to send selected txt file to server
        {
            string fileName = txtFile.Text; 
            Thread sendThread = new Thread(() => SendFile(fileName)); // SendFile(fileName) called
            sendThread.Start();
            string[] splitted = fileName.Split('\\');       //takes only the file name in a given path(txtFile.Text) to add to the message box(logs)
            string fName = splitted[splitted.Length - 1];
            logs.AppendText("File sent: " + fName + "\n");
        }

        public void SendFile(string fileName)
        {
            using (var file = File.OpenRead(fileName)) //opening the file
            {
                send_message("dummy");

                byte[] filename = Encoding.Default.GetBytes(fileName); //converts file name(taken as a variable) to byte

                int filenamesize = filename.Length;  //file name's size
                var filensize = BitConverter.GetBytes(filenamesize); //converting to byte 
                clientSocket.Send(filensize); //sending to the server
                
                clientSocket.Send(filename); //sending file name to server

                var fileSize = BitConverter.GetBytes((int)file.Length); //converting file's size 
                clientSocket.Send(fileSize); //sending to the server

                var sendBuffer = new byte[2048];
                var bytesLeftToTransmit = fileSize; //it is initially the whole file size, while sending buffers(sendBuffer) it will decrement.
                while (BitConverter.ToInt32(bytesLeftToTransmit, 0) > 0)
                {
                    var dataToSend = file.Read(sendBuffer, 0, sendBuffer.Length); //read inside of the file(to sendBuffer)
                    
                    int i = BitConverter.ToInt32(bytesLeftToTransmit, 0);
                    int sub = i-dataToSend;
                    byte[] sum = BitConverter.GetBytes(sub);
                    bytesLeftToTransmit = sum;
                    
                    //loop until the socket have sent everything in the buffer.
                    var offset = 0;
                    
                    while (dataToSend > 0)
                    {
                        var bytesSent = clientSocket.Send(sendBuffer, offset, dataToSend, SocketFlags.None);
                        dataToSend -= bytesSent;
                        offset += bytesSent;
                    }
                }
            }
        }

        private void button_disconnect_Click(object sender, EventArgs e) //disconnects the client
        {
            button_disconnect.Enabled = false;
            button_browse.Enabled = false;
            button_connect.Enabled = true;
            button_send.Enabled = false;
            logs.AppendText("You disconnected\n");
            clientSocket.Close();
            connected = false;
        }

        private void button_browse2_Click(object sender, EventArgs e) //select a folder for downloaded files
        {
            FolderBrowserDialog folderDlg = new FolderBrowserDialog();
            folderDlg.ShowNewFolderButton = true;
            DialogResult result = folderDlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                textBox_path.Text = folderDlg.SelectedPath;
                Environment.SpecialFolder root = folderDlg.RootFolder;
            }
            button_download.Enabled = true;//when folder path is given, dowload function enabled
            textBox_download.Enabled = true;
        }

        private void button_request_Click(object sender, EventArgs e) //request own file list from the server
        {
            send_message("I request my files");
        }

        private void button_delete_Click(object sender, EventArgs e) //delete own files on the server
        {
            send_message("Delete");
            string filename=textBox_delete.Text;
            send_message(filename); //sends the name of the file to be deleted
            //logs.AppendText("File deleted: " + filename + "\n");
        }

        private void button_download_Click(object sender, EventArgs e) //download her own files from the server
        {
            send_message("download");  //sends dowload request to the server
            string filename = textBox_download.Text;
            send_message(filename);   //sends the name of the file to be deleted
        }

        private void button_public_Click(object sender, EventArgs e) //send request for making files public for all clients
        {
            send_message("public");
            string filename = textBox_public.Text;
            send_message(filename); //sends the name of the file to made public
            //logs.AppendText("File made public: " + filename + "\n");
        }

        private void button_copy_Click(object sender, EventArgs e) //send request and file name for creating a copy of client's own file
        {
            send_message("copy");
            string filename = textBox_copy.Text;
            send_message(filename);
            //logs.AppendText("File copied: " + filename + "\n");

        }
    }
    
}
