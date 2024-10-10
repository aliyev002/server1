
using System.Net.Sockets;
using System.Net;

var ip = IPAddress.Parse("10.1.18.7");
var port = 27001;
var ep = new IPEndPoint(ip, port);
var listener = new TcpListener(ep);

listener.Start();
while (true)
{
    var client = listener.AcceptTcpClient();
    _ = Task.Run(() =>
    {
        var networkStream = client.GetStream();
        var remoteEp=client.Client.RemoteEndPoint as IPEndPoint;
        var directoryPath=Path.Combine(Environment.CurrentDirectory,remoteEp!.Address.ToString());

        if (Directory.Exists(directoryPath))
            Directory.CreateDirectory(directoryPath);

        var path = Path.Combine(directoryPath,$"{DateTime.Now:dd.MM.yyyy.HH.mm.ss}.png");

        using (var fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
        {
            int len = 0;
            var bytes = new byte[len];
            while ((len = networkStream.Read(bytes, 0, len)) > 0)
            {
                fs.Write(bytes, 0, len);
            }

        };
        Console.WriteLine("Files Received");
        client.Close();

    });
}


