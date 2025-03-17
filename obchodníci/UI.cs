using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace OOP_obchodníci 
{
    internal class UI
    {
        public Salesman currentSalesman;          
        List<Salesman> bosses = new List<Salesman>(); 

        public int currentUI = 1;
        public int curentMenu = 1; 
        public int firstMenuTile = 0;
        public int thirdMenuTile = 0;
        public bool saveFile = true;

        public string filesPath = "../../../lists/";
        public string currentFile = "";
        public List<int> currentFileSalesmen = new List<int>();

        public UI(Salesman node)
        {
            currentSalesman = node;
        }
        //vypisování stromu + metody z hodiny

        static void DisplaySalesmenTree(Salesman node, string indent = "")
        {
            Console.WriteLine($"{indent}{node.Name} {node.Surname} - Sales: {node.Sales}");

            foreach (var subordinate in node.Subordinates)
            {
                DisplaySalesmenTree(subordinate, indent + "    ");
            }
        }
        
        //najít uživatele rekurzí
        
        //menu pro ovládání 
        public void Render()
        {
            Console.Clear();
            if(currentUI==1)
            {
                RenderFirstMenu();
                RenderSecondMenu();
                RenderThirdMenu();
            }
            else if(currentUI ==2)
            {
                RenderFirstMenu2();
            }
            
        }
        // UI 2
        void RenderFirstMenu2()
        {
            string[] upMenu = { "Založit", "Načíst", "Uložit", "Přejít na prohlížeč" };
            int lineLength = 0;

            for (int i = 0; i < upMenu.Length; i++)
            {
                if (i == firstMenuTile && curentMenu == 1)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                }

                Console.Write(upMenu[i]);
                lineLength += upMenu[i].Length;

                Console.ResetColor();

                if (i != upMenu.Length - 1)
                {
                    Console.Write(" | ");
                    lineLength += 3;
                }
            }
            Console.WriteLine();
            Oddelovac(lineLength);

            if (currentFile != "")
            {
                Console.WriteLine("Seznam: " + currentFile);
                Oddelovac(8 + currentFile.Length);
            }

          
            if (firstMenuTile == 1)
            {
                string[] paths = Directory.GetFiles(filesPath);

                saveFile = false;
                for (int i = 0; i < paths.Length; i++)
                {
                    if (i == thirdMenuTile && curentMenu == 2)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.BackgroundColor = ConsoleColor.DarkBlue;
                    }

                    Console.WriteLine(paths[i].Substring(15));

                    Console.ResetColor();
                }
            }
        }

        public void Start()
        {
            Movement();
        }
        
        void RenderFirstMenu()
        {
            string[] upMenu = { "Přejít na nadřízeného", "Přejít nahoru", "Přejít na seznam" };
            int lineLength = 0;

            for (int i = 0; i < upMenu.Length; i++)
            {
                if (i == firstMenuTile && curentMenu == 1)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                }

                Console.Write(upMenu[i]);
                lineLength += upMenu[i].Length;

                Console.ResetColor();

                if (i != upMenu.Length - 1)
                {
                    Console.Write(" | ");
                    lineLength += 3;
                }
            }

            Console.WriteLine();
            Oddelovac(lineLength);
        }

        void RenderSecondMenu()
        {
            Console.WriteLine();
            Console.Write("Obchodník: " + currentSalesman.Name + " " + currentSalesman.Surname);

            Console.Write("  ");
            if(curentMenu ==2)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.BackgroundColor = ConsoleColor.DarkBlue;
            }

            if (currentFile == "" || !currentFileSalesmen.Contains(currentSalesman.ID))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Přidat");
                Console.ResetColor();
            }
            else if (currentFile != "" && currentFileSalesmen.Contains(currentSalesman.ID))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Odebrat");
                Console.ResetColor();
            }

            Console.ResetColor();

            Oddelovac(12 + currentSalesman.Name.Length + currentSalesman.Surname.Length);

            Console.WriteLine("Přímé prodeje: " + currentSalesman.Sales + " $");
            Console.WriteLine("Celkové prodeje sítě: " + currentSalesman.TotalSales(currentSalesman) + " $");
        }

        void RenderThirdMenu()
        {
            Console.WriteLine();

            if (bosses.Any())
            {
                Console.Write("Nadřízený: ");
                Console.WriteLine(bosses.Last().Name + " " + bosses.Last().Surname);
                Console.WriteLine();
            }

            if (currentSalesman.Subordinates.Any())
            {
                Console.Write("Podřízení: ");

                for (int i = 0; i < currentSalesman.Subordinates.Count; i++)
                {
                    if (i == thirdMenuTile && curentMenu == 3)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.BackgroundColor = ConsoleColor.DarkBlue;
                    }

                    Console.WriteLine(currentSalesman.Subordinates[i].Name + " " + currentSalesman.Subordinates[i].Surname);
                    Console.ResetColor();
                    Console.Write("           ");
                }
            }
        }

        //pohyb v pohybu bude načítat klavesi pri stisknutí e se zeptá jestli chce uživatek ukoncit a pripadně ulozit svoji praci
        void Movement()
        {
            while (true)
            {
                Render();

                var key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.E)
                {
                    end();
                }
                if(currentUI ==1)
                {
                    switch (key)
                    {
                        case ConsoleKey.UpArrow:
                            thirdMenuTile--;
                            if (thirdMenuTile < 0)
                            {
                                thirdMenuTile = currentSalesman.Subordinates.Count - 1;
                            }
                            break;
                        case ConsoleKey.DownArrow:
                            thirdMenuTile++;
                            if (thirdMenuTile > currentSalesman.Subordinates.Count - 1)
                            {
                                thirdMenuTile = 0;
                            }
                            break;
                        case ConsoleKey.RightArrow:
                            firstMenuTile++;
                            if (firstMenuTile > 2)
                            {
                                firstMenuTile = 0;
                            }
                            break;
                        case ConsoleKey.LeftArrow:
                            firstMenuTile--;
                            if (firstMenuTile < 0)
                            {
                                firstMenuTile = 2;
                            }
                            break;
                        case ConsoleKey.Spacebar:
                            curentMenu++;
                            if (curentMenu > 3 || (curentMenu == 3 && !currentSalesman.Subordinates.Any()))
                            {
                                curentMenu = 1;
                            }
                            firstMenuTile = 0;
                            thirdMenuTile = 0;

                            break;
                        case ConsoleKey.Enter:
                            if (curentMenu == 1)
                            {
                                switch (firstMenuTile)
                                {
                                    case 0:
                                        if (bosses.Any())
                                        {
                                            currentSalesman = bosses.Last();
                                            bosses.Remove(currentSalesman);
                                        }
                                        break;
                                    case 1:
                                        if (bosses.Any())
                                        {
                                            currentSalesman = bosses.First();
                                            bosses.Clear();
                                        }
                                        break;
                                    case 2:
                                        currentUI = 2;
                                        curentMenu = 1;
                                        firstMenuTile = 0;
                                        thirdMenuTile = 0;
                                        break;
                                }
                            }
                            else if (curentMenu == 2 && currentFile != "")
                            {
                                saveFile = false;
                                if (!currentFileSalesmen.Contains(currentSalesman.ID))
                                {
                                    currentFileSalesmen.Add(currentSalesman.ID);
                                }
                                else
                                {
                                    currentFileSalesmen.RemoveAll(n => n == currentSalesman.ID);
                                }
                            }
                            else if (curentMenu == 3)
                            {
                                bosses.Add(currentSalesman);
                                currentSalesman = currentSalesman.Subordinates[thirdMenuTile];
                                curentMenu = 1;
                            }
                            break;
                    }
                }
                else
                {
                    switch (key)
                    {
                        case ConsoleKey.LeftArrow:
                            if (curentMenu == 1)
                            {
                                firstMenuTile--;
                                if (firstMenuTile < 0)
                                {
                                    firstMenuTile = 3;
                                }
                               
                            }
                            break;
                        case ConsoleKey.RightArrow:
                            if(curentMenu==1)
                            {
                                firstMenuTile++;
                                if (firstMenuTile > 3)
                                {
                                    firstMenuTile = 0;
                                }
                            }
                            break;
                        case ConsoleKey.Enter:
                            if (curentMenu == 1)
                            {
                                switch (firstMenuTile)
                                {
                                    case 0:
                                        NewFile();
                                        break;
                                    case 1:
                                        curentMenu = 2;
                                        thirdMenuTile = 0;
                                        break;
                                    case 2:
                                        SaveFile();
                                        break;
                                    case 3:
                                        currentUI = 1;
                                        curentMenu = 1;
                                        firstMenuTile = 0;
                                        thirdMenuTile = 0;
                                        break;
                                }
                            }
                            else if(curentMenu == 2)
                            {
                                string[] paths = Directory.GetFiles(filesPath);
                                currentFile = paths[thirdMenuTile].Substring(15);
                                curentMenu = 1;

                                currentFileSalesmen = File.ReadAllLines(filesPath + currentFile)
                                    .Select(int.Parse)
                                    .ToList();
                            }
                            break;
                        case ConsoleKey.UpArrow:
                            thirdMenuTile--;
                            if (thirdMenuTile < 0)
                            {
                                thirdMenuTile = Directory.GetFiles(filesPath).Length - 1;
                            }
                            break;
                        case ConsoleKey.DownArrow:
                            thirdMenuTile++;
                            if (thirdMenuTile > Directory.GetFiles(filesPath).Length - 1)
                            {
                               thirdMenuTile = 0;
                            }
                            break;
                    }

                }




            }
        }

        static void Oddelovac(int length)
        {
            string oddelovac = "";

            for (int i = 0; i < length; i++)
            {
                oddelovac += "-";
            }

            Console.WriteLine(oddelovac);
        }
        
        //zalozeni
        void NewFile()
        {
           
            Console.Write("Zadej jmeno souboru bez koncovky: ");
            currentFile = Console.ReadLine() + ".txt";
            File.Create(filesPath + currentFile);
            saveFile = false;
        }


        //uložení
        void SaveFile()
        {
            if (currentFile != "")
            {
                saveFile = true;

                using (StreamWriter writer = new StreamWriter(filesPath + currentFile))
                {
                    foreach (int number in currentFileSalesmen)
                    {
                        writer.WriteLine(number);
                    }
                }
            }
        }



        //ukončení pomocí "end"        //zavřít i přes neuložený seznam
        void end()
        {
            Console.Clear();
            if (!saveFile)
            {
                Console.WriteLine("nemáte uložený seznam chcete uložit? (A/N)");
                while (true)
                {
                    string input = Console.ReadLine();
                    if (input == "A" || input == "a")
                    {
                        SaveFile();
                        break;
                    }
                    else if (input == "N" || input == "n")
                    {
                        Console.WriteLine("program se ukončí bez uložení");
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Špatný vstup");
                    }
                }
            }
            Environment.Exit(0);
        }


    }
}
