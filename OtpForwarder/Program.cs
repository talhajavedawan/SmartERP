using System.IO.Ports;
using System.Management;


// See https://aka.ms/new-console-template for more information
Console.WriteLine("Service starting");
ManagementObject selectedPort;
ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_POTSModem");
List<string> portsAvailable = new List<string>();

Console.WriteLine("\n\nConnected Deviced List:-------------------");
foreach (ManagementObject port in searcher.Get())
{
    Console.ForegroundColor = ConsoleColor.Blue;
    if (port.Properties["Status"].Value.ToString() == "OK")
    {
        portsAvailable.Add(port.Properties["StatusInfo"].Value.ToString());
        Console.WriteLine(port.Properties["ResponsesKeyName"].Value + "[" + port.Properties["StatusInfo"].Value + "]");
    }
}
Console.ForegroundColor = ConsoleColor.White;
Console.WriteLine("------------------------------------------");

recheck:
Console.WriteLine("\n\nPlease Enter port number from list, Press Enter: >>");
string portNumber = (Console.ReadLine());

if (portNumber !=null && portsAvailable.Contains(portNumber) == false)
    goto recheck;

SerialPort serialport = new SerialPort();
int mSpeed = 1;
serialport.PortName = "COM" + Convert.ToInt16(portNumber.Trim()).ToString("00"); // "COM3";
serialport.BaudRate = 9600;
serialport.Parity = Parity.None;
serialport.DataBits = 8;
serialport.StopBits = StopBits.One;
serialport.Handshake = Handshake.XOnXOff;
serialport.DtrEnable = true;
serialport.RtsEnable = true;
serialport.NewLine = Environment.NewLine;

serialport.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);

serialport.DtrEnable = true;
serialport.RtsEnable =true;
serialport.Open();
Console.WriteLine("Connection Established with Modem at >> " + serialport.PortName);
while (true)
{
    Console.WriteLine("checking new message");
    readMessage(serialport);
    System.Threading.Thread.Sleep(5000);
}

void readMessage(SerialPort serialport)
{
    Console.WriteLine("1");
    serialport.Write("AT" + System.Environment.NewLine);

    Thread.Sleep(1000);
    Console.WriteLine("2");
    serialport.WriteLine("AT+CMGF=1" + System.Environment.NewLine);
    Console.WriteLine("3");
    Thread.Sleep(1000);
    Console.WriteLine("4");
    //serialport.WriteLine("AT+CMGF=\"ALL\"\r" + System.Environment.NewLine);
    serialport.WriteLine("AT+CMGF=\"REC UNREAD\"" + System.Environment.NewLine);
    Console.WriteLine("5");
    Thread.Sleep(3000);
    Console.WriteLine("6");
    Console.WriteLine(">>" + serialport.ReadExisting());
    Console.WriteLine("7");
}

//serialport.Write("AT+CMGF=1\r");
//serialport.Write("AT+CNMI=1,2,0,0,0\r");

//serialport.WriteLine("$"); //Command to start Data Stream

//Console.ReadLine();

//serialport.WriteLine("!"); //Stop Data Stream Command



Console.ReadKey();
Console.WriteLine("Closing connection.");
serialport.Close();


static void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
{

    SerialPort sp = (SerialPort)sender;


    //SerialPort sp = (SerialPort)sender;
    //sp.WriteLine("AT+CMTI" + Environment.NewLine);
    //Console.WriteLine("Message recieved >>" + sp.ReadLine());

    //SerialPort sp = (SerialPort)sender;
    //string input = sp.ReadExisting();
    //Console.WriteLine(input);

    //SerialPort serPort = (SerialPort)sender;
    //string input = serPort.ReadExisting();

    //if (input.Contains("+CMT:"))
    //{

    //    if (input.Contains("AT+CMGF=1"))
    //    {
    //        string[] message = input.Split(Environment.NewLine.ToCharArray()).Skip(7).ToArray();
    //        Console.WriteLine( string.Join(Environment.NewLine, message));
    //    }
    //}
    //else
    //{
    //    return;
    //}


}
