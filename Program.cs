using System;
using System.Collections.Generic;

namespace ConsoleApp
{
    public class Program
    {

        public class Meni
        {
            string menuName;
            int parentID;
            bool isHidden;
            bool isRootMenu;
            int id;
            int level;
            List<Meni> children = new List<Meni>();
            public Meni(string mmenuName, int ID, int mparentID, bool misHidden, bool misRootMenu, int mlevel)
            {
                menuName = mmenuName;
                parentID = mparentID;
                isHidden = misHidden;
                isRootMenu = misRootMenu;
                id = ID;
                level = mlevel;
            }

            public void addChild(Meni childToAdd)
            {
                children.Add(childToAdd);
            }
            public void sortTheChildren()
            {
                children.Sort(delegate (Meni x, Meni y) {
                    return x.menuName.CompareTo(y.menuName);
                });
            }
            public List<Meni> childList { get => children; set => children = value; }
            public string MenuName { get => menuName; set => menuName = value; }
            public int ParentID { get => parentID; set => parentID = value; }
            public bool IsHidden { get => isHidden; set => isHidden = value; }
            public bool IsRootMenu { get => isRootMenu; set => isRootMenu = value; }
            public int Id { get => id; set => id = value; }
            public int Level { get => level; set => level = value; }

        }

        static void Printingfunc(Meni meni)
        {
            Meni rMeni = meni;
            if (rMeni.childList.Count != 0)
            {
                string indentationLevel = String.Concat(System.Linq.Enumerable.Repeat("...", rMeni.Level - 1));
                if (rMeni.Level == 2)
                {
                    indentationLevel = ".";
                }
                Console.WriteLine(indentationLevel + rMeni.MenuName);
                List<Meni> children = meni.childList;
                foreach (Meni child in children)
                {
                    if (child.childList.Count != 0)
                    {
                        Printingfunc(child);
                    }
                    else
                    {
                        if (!child.IsHidden)
                        {
                            indentationLevel = String.Concat(System.Linq.Enumerable.Repeat("...", child.Level - 1));
                            Console.WriteLine(indentationLevel + child.MenuName);
                        }
                    }
                }
            }
        }
        public static List<Meni> LoadCSV(string filename)
        {
            string whole_file = System.IO.File.ReadAllText(filename);

            string[] lines = whole_file.Split(new char[] { '\n' },
                StringSplitOptions.RemoveEmptyEntries);

            List<Meni> allMenus = new List<Meni>();

            int num_rows = lines.Length;
            int num_cols = lines[0].Split(';').Length;
            string[] value;
            bool isroot;
            int indentation;
            for (int l = 1; l < lines.Length; l++)
            {
                value = lines[l].Split(";");
                if (value[2] == "NULL")
                {
                    isroot = true;
                }
                else
                {
                    isroot = false;
                }
                indentation = value[4].Split("/").Length;
                string name = value[1];
                string idstr = value[0];
                int id = Int32.Parse(idstr);
                string parentIdstr = value[2];
                int parentId = 0;
                if (parentIdstr != "NULL")
                {
                    parentId = Int32.Parse(parentIdstr);
                }
                string isHiddenstr = value[3];
                bool isHidden = bool.Parse(isHiddenstr);
                Meni newMeni = new Meni(name, id, parentId, isHidden, isroot, indentation);

                allMenus.Add(newMeni);
            }

            for (int m = 0; m < allMenus.Count; m++)
            {
                for (int i = 1; i < lines.Length; i++)
                {

                    value = lines[i].Split(";");
                    string parIdstr = value[2];
                    if (parIdstr != "NULL")
                    {
                        int parid = Int32.Parse(parIdstr);
                        if (parid == allMenus[m].Id)
                        {
                            allMenus[m].addChild(allMenus[i - 1]);
                        }
                    }
                }
                allMenus[m].sortTheChildren();
            }

            return allMenus;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Please enter the path and filename, for example: C:\\Users\\user\\Navigation.csv");

            string sampleCSV = @"";
            sampleCSV = sampleCSV + Console.ReadLine();

            List<Meni> menus = LoadCSV(sampleCSV);
            foreach (Meni menu in menus)
            {
                if (menu.IsRootMenu)
                {
                    Printingfunc(menu);
                }
            }
            Console.ReadLine();
        }
    }
}
