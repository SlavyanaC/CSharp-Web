namespace _02SliceFile
{
    using System;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;

    class StartUp
    {
        static void Main(string[] args)
        {
            string videoPath = "../../../video.mp4";
            string destinationPath = "../../../Pieces/";
            int pieces = int.Parse(Console.ReadLine());

            //Slice(videoPath, destinationPath, pieces);

            SliceAsync(videoPath, destinationPath, pieces);

            Console.WriteLine("Anything else?");
            while (true)
            {
                Console.ReadLine();
            }
        }

        private static void SliceAsync(string videoPath, string destinationPath, int pieces)
        {
            Task.Run(() =>
            {
                Slice(videoPath, destinationPath, pieces);
            });
        }

        private static void Slice(string videoPath, string destinationPath, int pieces)
        {
            if (!Directory.Exists(destinationPath))
            {
                Directory.CreateDirectory(destinationPath);
            }

            Thread.Sleep(10000);

            using (FileStream source = new FileStream(videoPath, FileMode.Open))
            {
                FileInfo fileInfo = new FileInfo(videoPath);

                long partLength = (source.Length / pieces) + 1;
                long currentByte = 0;

                for (int currentPart = 1; currentPart <= pieces; currentPart++)
                {
                    string filePath = string.Format("{0}/Part-{1}{2}", destinationPath, currentPart, fileInfo.Extension);

                    using (var destination = new FileStream(filePath, FileMode.Create))
                    {
                        byte[] buffer = new byte[1024];
                        while (currentByte <= partLength * currentPart)
                        {
                            int readBytesCount = source.Read(buffer, 0, buffer.Length);

                            if (readBytesCount == 0)
                            {
                                break;
                            }

                            destination.Write(buffer, 0, readBytesCount);
                            currentByte += readBytesCount;
                        }

                    }
                }

                Console.WriteLine("Slice complete.");
            }
        }
    }
}
