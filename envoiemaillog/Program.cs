using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace envoiemaillog
{
    class Program
    {
        static void Main(string[] args)
        {
            string content = "";
            string P1 = args[0]; //appel du nom de dossier et le stocker dans P1
            string path = args[0];
            //int position = P1.LastIndexOf("\\");
            P1 = P1.Substring(P1.LastIndexOf("\\") + 1);


            FileInfo fi = new FileInfo(@"" + args[0] + "\\LogsOF.txt");// on stock les infos du fichier LogsOF dans la varable fi
            FileInfo fic = new FileInfo(@"" + args[0] + "\\logtoolsSplit_Bj_Files.txt");

            DateTime a = fi.LastWriteTime;//la variable a contient l'heure de modfif du fichier LogsOF
            DateTime b = fic.LastWriteTime;

            string[] tab = File.ReadAllLines(@"modif.txt");// On stock dans tab ce que contient le doc.txt

            string x;// création d'une variable qui nous permet de stocker la dernière heure de modfication d'un des deux fichiers




            if (a < b)
            {
                x = b.ToString();// si l'heure de modif de LogsOF.txt est inférieur à celle de logtoolsSplit_Bj_Files.txt alors la variable x prend la valeur de b
            }
            else
            {
                x = a.ToString();
            }



            int i = 0;
            for (i = 0; i < tab.Length; i++)
            {
                string val = tab[i];
                string taille = val.Substring(val.LastIndexOf(";") + 1);


                if (P1 == taille)
                {
                    content = val.Substring(0, val.LastIndexOf(";"));


                    if (x != content)
                    {
                        tab[i] = (x + ";" + taille);


                    }


                }


            }





            if (x != content)
            {





                File.Delete(@"modif.txt");
                File.WriteAllLines(@"modif.txt", tab);// si l'heure de modif d'un des deux fichiers est différente de celle qu'on a sauvegardé alors on la remplace à la place de celle que l'on avait sauvegardé dans le fichier modifopenflux.txt
                try// envoie du mail
                {
                    MailMessage email = new MailMessage();
                    email.To.Add("skallyx19@gmail.com");

                    //email.CC.Add("ochassing@gmail.com");
                    email.From = new MailAddress("skallyx19@gmail.com");
                    email.Subject = "Test";
                    email.Body = "coucou ça marche";
                    System.Net.Mail.Attachment attachment;
                    System.Net.Mail.Attachment attachment2;
                    attachment = new System.Net.Mail.Attachment(args[0] + "\\LogsOF.txt");
                    attachment2 = new System.Net.Mail.Attachment(args[0] + "\\logtoolsSplit_Bj_Files.txt");
                    email.Attachments.Add(attachment);
                    email.Attachments.Add(attachment2);
                    email.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.gmail.com";
                    smtp.Credentials = new System.Net.NetworkCredential("", "");
                    smtp.Port = 587;

                    smtp.EnableSsl = true;
                    smtp.Send(email);
                }
                catch (Exception ex)
                {
                    string[] contents = { ("Exception in sendEmail:" + ex.Message) };
                }
            }
        }
    }
}
