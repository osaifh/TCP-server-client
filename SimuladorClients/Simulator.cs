using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SimuladorClients
{
    class Simulator
    {
        static Queue<Process> cua_processos = new Queue<Process>();
        
        static void Main(string[] args)
        {
            Thread thread = new Thread(() => taskManager());
            thread.Start();
            string opcio = "";
            while (opcio != "q")
            {
                Console.WriteLine("Crear nou client (c), sortir(q)");
                opcio = Console.ReadLine();

                if (opcio == "c")
                {
                    generar_proces();
                }
            }
        }

        private static void generar_proces()
        {

            //Aqui cal fer el següent:
            //1) Crear un nou process d'EscriureLletra amb les dades que ens ha passat l'usuari com a arguments 
            Process P = new Process();
            P.StartInfo.FileName = @"..\..\..\Client\bin\Debug\Client";
            P.EnableRaisingEvents = true;
            //P.StartInfo.Arguments = lletra + " " + vegades + " " + retard;
            P.Exited += new EventHandler(myProces_Exited);

            //2) Guardar el procés a la cua
            cua_processos.Enqueue(P);
        }

        static void taskManager()
        {
            while (true)
            {
                if (cua_processos.Count != 0)
                {
                    Process p = cua_processos.Dequeue();
                    p.Start();
                }
            }
        }

        private static void myProces_Exited(object sender, System.EventArgs e)
        {
            Console.WriteLine("Client desconectat");
        }
    }


}
