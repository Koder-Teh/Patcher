using Patcher.Interface;
using Patcher.Other;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

namespace Patcher.Patching
{
    internal class HandlerFile : IEnviroment
    {
        public async Task ProcessHandler()
        {
            List<string> text = new List<string>();
            using (FileStream stream = new FileStream(GetInstante()._args.file.FullName, FileMode.Open, FileAccess.ReadWrite))
            {
                for (int i = 0; i < GetInstante()._template.patchs.Count; i++)
                {
                    Patchs patch = GetInstante()._template.patchs[i];

                    if (!GetInstante()._args.options_mask)
                    {
                        if (Hex.DecimalFormat(patch.offset) < stream.Length)
                        {
                            Console.WriteLine();
                            Console.WriteLine($"============================================");
                            Console.WriteLine($"Offset: {patch.offset}");
                            Console.WriteLine($"Restore: {patch.restore}");
                            Console.WriteLine($"Patch: {patch.patch}");
                            Console.WriteLine($"============================================");
                            Console.WriteLine();

                            stream.Seek(Convert.ToInt64(patch.offset, 16), SeekOrigin.Begin);
                            if (patch.restore.Length > 2)
                            {
                                List<byte> patchBytes = new List<byte>();
                                foreach (string @byte in patch.patch.Split(' '))
                                {
                                    patchBytes.Add(Convert.ToByte(@byte, 16));
                                }
                                byte[] bytesPatch = new byte[patchBytes.Count];
                                patchBytes.CopyTo(bytesPatch);
                                stream.Write(bytesPatch, 0, bytesPatch.Length);
                            }
                            else
                            {
                                stream.WriteByte(Convert.ToByte(patch.patch, 16));
                            }
                        }
                    }
                    else
                    {
                        #pragma warning disable CA2022
                        List<int> listpattern = new List<int>();
                        int wishlide = 0;
                        foreach (string b in patch.mask.Split(' '))
                        {
                            if (!b.Contains("?")) listpattern.Add(Hex.DecimalFormat(b));
                            else
                            {
                                listpattern.Add(-1);
                                wishlide++;
                            }
                        }

                        Console.WriteLine($"Mask: {string.Join(' ', listpattern.Select(n =>
                        {
                            if (n == -1)
                            {
                                return "??";
                            }
                            else
                            {
                                return n.ToString("X2");
                            }
                        }))}");

                        byte[] bytes = new byte[GetInstante()._template.buffer_length];
                        int stoppos = 0;

                        while (true)
                        {
                            stream.Seek(stoppos, SeekOrigin.Begin);
                            int n = stream.Read(bytes, 0, bytes.Length);

                            if (n == 0) break;

                            for (int y = 0; y < bytes.Length; y++)
                            {
                                int p = 0;
                                if ((y + listpattern.Count) > bytes.Length)
                                {
                                    stoppos += y;
                                    break;
                                }

                                for (int x = 0; x < listpattern.Count; x++)
                                {
                                    if (listpattern[x] == -1) continue;
                                    if (listpattern[x] == bytes[y + x])
                                    {
                                        ++p;
                                    }
                                }

                                if (p == (listpattern.Count - wishlide))
                                {
                                    text.Add(String.Format("[+] {0:X}", stoppos + y));
                                }
                            }
                        }

                        Console.Clear();

                        #pragma warning restore CA2022
                    }
                }
            }

            foreach (string findAddress in text)
            {
                Console.WriteLine(findAddress);
            }

            Console.WriteLine("Success Patch!");
        }

        public Enviroment GetInstante()
        {
            return Program.Enviroment;
        }
    }
}
