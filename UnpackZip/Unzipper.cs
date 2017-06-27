using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnpackZip
{
    

    public class FileInZip
    {
        public Byte[] localFileHeaderSignature = new Byte[4];
        public Byte[] versionNeededToExtract = new Byte[4];
        public Byte[] generalPurposeBitFlag = new Byte[2];
        public Byte[] compressionMethod = new Byte[2];
        public Byte[] fileLastModificationTime = new Byte[2];
        public Byte[] fileLastModificationDate = new Byte[2];
        public Byte[] CRC32 = new Byte[4];
        public Byte[] compressedSize = new Byte[4];
        public Byte[] uncompressedSize = new Byte[4];
        public Byte[] fileNameLength = new Byte[2];
        public Byte[] extraFieldLength = new Byte[2];
        public Byte[] fileName;
        public Byte[] extraField;
        public Byte[] fileData;
        public int byteDataStarts;
        public int compressedSizeInt;
        public int uncompressedSizeInt;
        public string name;
        public int nextFileByte;
    }

    public class Zipper
    {
        public string filePath;
        string srcName;
        string fileName;
        public Byte[] fileBytecode;
        public Byte[] byteArray;
        public FileInZip[] arrFiles;
        public int filesCounter = 0;
        List<int> indexOfFile = new List<int>();

        public Zipper(string path)
        {
            srcName = path;
            filePath = path;
            fileName = Path.GetFileName(path);
            byteArray = File.ReadAllBytes(srcName);
            filesCounter = howManyFilesInZip();
            arrFiles = new FileInZip[filesCounter];
            for (int p = 0; p < filesCounter; ++p)
            {
                arrFiles[p] = new FileInZip();
                if (p == 0)
                    ByteCodeToArrays(p, arrFiles[p]);
                else
                    ByteCodeToArrays(arrFiles[p - 1].nextFileByte, arrFiles[p]);
            }
        }



        public int howManyFilesInZip()
        {
            for (int i = 0; i < byteArray.Length - 4; i++)
            {
                string testSignature = byteArray[i].ToString() + byteArray[i + 1].ToString() + byteArray[i + 2].ToString() + byteArray[i + 3].ToString();
                if (testSignature == "807534")
                {
                    indexOfFile.Add(i);
                    filesCounter++;
                }

            }

            return filesCounter;
        }

        public void ByteCodeToArrays(int startByte, FileInZip fiz)
        {
            Array.Copy(byteArray, startByte + 0, fiz.localFileHeaderSignature, 0, 4);
            Array.Copy(byteArray, startByte + 4, fiz.versionNeededToExtract, 0, 2);
            Array.Copy(byteArray, startByte + 6, fiz.generalPurposeBitFlag, 0, 2);
            Array.Copy(byteArray, startByte + 8, fiz.compressionMethod, 0, 2);
            int compressionMethodInt = BitConverter.ToInt16(fiz.compressionMethod, 0);
            Console.WriteLine("Metoda kompresji: " + compressionMethodInt);
            Array.Copy(byteArray, startByte + 10, fiz.fileLastModificationTime, 0, 2);
            Array.Copy(byteArray, startByte + 12, fiz.fileLastModificationDate, 0, 2);
            Array.Copy(byteArray, startByte + 14, fiz.CRC32, 0, 4);
            Array.Copy(byteArray, startByte + 18, fiz.compressedSize, 0, 4);
            fiz.compressedSizeInt = BitConverter.ToInt32(fiz.compressedSize, 0);
            Array.Copy(byteArray, startByte + 22, fiz.uncompressedSize, 0, 4);
            fiz.uncompressedSizeInt = BitConverter.ToInt32(fiz.uncompressedSize, 0);
            Array.Copy(byteArray, startByte + 26, fiz.fileNameLength, 0, 2);
            var nameLength = BitConverter.ToInt16(fiz.fileNameLength, 0);
            Array.Copy(byteArray, startByte + 28, fiz.extraFieldLength, 0, 2);
            var extraLength = BitConverter.ToInt16(fiz.extraFieldLength, 0);
            fiz.fileName = new Byte[nameLength];
            fiz.extraField = new Byte[extraLength];
            Array.Copy(byteArray, startByte + 30, fiz.fileName, 0, nameLength);
            Console.WriteLine(fiz.compressedSizeInt);
            //Console.WriteLine(fiz.uncompressedSizeInt);
            fiz.name = Encoding.ASCII.GetString(fiz.fileName);
            Array.Copy(byteArray, startByte + 30 + nameLength, fiz.extraField, 0, extraLength);
            fiz.fileData = new Byte[fiz.compressedSizeInt];
            Array.Copy(byteArray, startByte + 30 + nameLength + extraLength, fiz.fileData, 0, fiz.compressedSizeInt);
            fiz.byteDataStarts = startByte + 30 + nameLength + extraLength;
            fiz.nextFileByte = startByte + 30 + nameLength + extraLength + fiz.compressedSizeInt;
            //Console.WriteLine(Encoding.ASCII.GetString(fileData));
        }
    }
}
