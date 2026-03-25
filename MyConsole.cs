using System.Text;

namespace TMP_Laba2
{
    public class MyConsole
    {
        private const string startCommandLine = "VM>";
        private const string paramNotFoundExceptionText = "Не удалось найти подходящий параметр!";
        private const string paramNotExistsExceptionText = "У данной команды отсутствуют параметры!";
        private const string commandNotFoundExceptionText = "Команда не найдена!";

        public static void StartConsole()
        {
            string? commandLineText;
            ConsoleCommands commands = new ConsoleCommands();
            while (true)
            {
                Console.Write(startCommandLine);
                commandLineText = Console.ReadLine();

                if (commandLineText == null || commandLineText == "")
                    continue;
                var commandText = commandLineText.Split();
                try
                {
                    switch (commandText[0])
                    {
                        case "Create":
                            if (commandText.Length == 3)
                                commands.Create(commandText[1], commandText[2].ToArrayType());
                            else if (commandText.Length == 4)
                                commands.Create(commandText[1], commandText[2].ToArrayType(), Convert.ToInt32(commandText[3]));
                            else if (commandText.Length == 5)
                                commands.Create(commandText[1], commandText[2].ToArrayType(), Convert.ToInt32(commandText[3]), Convert.ToInt32(commandText[4]));
                            else
                                throw new ArgumentException(paramNotFoundExceptionText);
                            break;

                        case "Open":
                            if (commandText.Length != 2)
                                throw new ArgumentException(paramNotFoundExceptionText);
                            commands.Open(commandText[1]);
                            break;

                        case "Input":
                            if (commandText.Length < 3)
                                throw new ArgumentException(paramNotFoundExceptionText);

                            string value = string.Join(" ", commandText.Skip(2));

                            commands.Input(commandText[1], value);
                            break;

                        case "Print":
                            if (commandText.Length != 2)
                                throw new ArgumentException(paramNotFoundExceptionText);
                            else
                                commands.Print(Convert.ToInt32(commandText[1]));
                            break;

                        //case "Help":
                        //    if (commandText.Length > 2)
                        //        throw new ArgumentException(paramNotFoundExceptionText);
                        //    if (commandText.Length == 1)
                        //        commands.Help();
                        //    else if (commandText.Length == 2)
                        //        commands.Help(commandText[1]);
                        //    break;

                        case "Exit":
                            if (commandText.Length != 1)
                                throw new ArgumentException(paramNotExistsExceptionText);
                            commands.Exit();
                            return;

                        //case "Test":
                        //    if (commandText.Length != 1)
                        //        throw new ArgumentException(paramNotExistsExceptionText);
                        //    commands.Test();
                        //    break;
                        default:
                            throw new ArgumentException(commandNotFoundExceptionText);

                    }
                }
                catch (NotImplementedException)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Команда разрабатывается!");
                    Console.ResetColor();
                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Ошибка: " + e.Message);
                    Console.ResetColor();
                }
            }
        }
    }

    /// <summary>
    /// Команды для консоли
    /// </summary>
    ///  : IDisposable
    public class ConsoleCommands
    {
        private FileManager? manager;
        private string path = @$"C:\Users\{Environment.UserName}\Downloads\";
        private const string fileNotFoundExc = "Для начала нужно создать или открыть файл!";

        private bool CheckFilename(string filename)
        {
            if (filename.EndsWith(".prd") && filename.Length <= 16)
                return true;
            return false;
        }

        public void Create(string filename, ArrayType arrayType, int length = 10000)
        {
            //if (!CheckFilename(filename))
            //    throw new Exception("Нельзя создать файл с заданным расширением!");

            if (manager != null)
                manager.Dispose();

            if (File.Exists(path + filename))
            {
                while (true)
                {
                    Console.WriteLine("Файл уже создан. Открыть его? (Д/н)");
                    string? ans = Console.ReadLine();
                    if (ans == "Д")
                    {
                        manager = FileManager.OpenFiles(filename);
                        break;
                    }
                    else if (ans == "н")
                    {
                        return;
                    }
                    else
                    {
                        continue;
                    }
                }
                return;
            }

            if (arrayType == ArrayType.String)
                throw new Exception("Для создания массива с заданным типом введите дополнительные параметры!");
            else if (arrayType == ArrayType.Int)
                manager = FileManager.CreateIntArrayFiles(filename, length);
            else if (arrayType == ArrayType.Char)
                manager = FileManager.CreateCharArrayFiles(filename, length);
            else
                throw new Exception("Тип массива задан неверно!");

        }

        public void Create(string filename, ArrayType arrayType, int charCount, int length)
        {
            //if (!CheckFilename(filename))
            //    throw new Exception("Нельзя создать файл с заданным расширением!");

            if (manager != null)
                manager.Dispose();

            if (File.Exists(path + filename))
            {
                while (true)
                {
                    Console.WriteLine("Файл уже создан. Открыть его? (Д/н)");
                    string? ans = Console.ReadLine();
                    if (ans == "Д")
                    {
                        manager = FileManager.OpenFiles(filename);
                        break;
                    }
                    else if (ans == "н")
                    {
                        return;
                    }
                    else
                    {
                        continue;
                    }
                }
                return;
            }


            if (arrayType == ArrayType.String)
                manager = FileManager.CreateStringArrayFiles(filename, charCount, length);
            else
                throw new Exception("Неверное количество параметров для заданного типа!");
        }

        public void Open(string filename) 
        {
            if (manager != null)
                manager.Dispose();

            if (!File.Exists(path + filename))
                throw new Exception("Файл не найден!");

            manager = FileManager.OpenFiles(filename);
        }

        /// <summary>
        /// Команда закрывает все файлы и завершает программу.
        /// </summary>
        public void Exit()
        {
            manager?.Dispose();
        }

        /// <summary>
        /// Команда выводит на экран или в указанный файл список команд.
        /// </summary>
        /// <param name="filename">Имя файла</param>
        //public void Help(string? filename = null)
        //{
        //    StringBuilder help = new StringBuilder();
        //    help.Append("Create (имя файла, [максимальная длина имени компонента], [имя файла спецификаций]) — если файл существует и сигнатура соответствует заданию, команда требует\n" +
        //        "подтверждения на перезапись файла. При положительном ответе, файлы очищаются, после чего создаются все необходимые структуры в памяти и файлах на диске.\n" +
        //        "После успешного выполнения команды файлы считаются открытыми для работы. Если сигнатура файла отсутствует или не соответствует заданию, команда вызывает ошибку.\n" +
        //        "Расширение имени файла для списка компонентов — «.prd»., а для файла спецификаций — «.prs».\n\n");
        //    help.Append("Open (имя файла) — открывает указанный файл и связанные с ним файлы в режиме rw, создает все необходимые структуры в памяти.\n" +
        //        "Если сигнатура файла отсутствует или несоответствует заданию, команда вызывает ошибку.\n\n");
        //    help.Append("Input (имя компонента, тип) — включает компонент в список. тип — одно из следующего: Изделие, Узел, Деталь.\n\n");
        //    help.Append("Input (имя компонента/имя комплектующего) — включает комплектующее в спецификацию компонента. Имя комплектующего должно быть в списке,\n" +
        //        "в противном случае и для детали эта команда вызывает ошибку.\n\n");
        //    help.Append("Print (имя компонента) — выводит на экран состав компонента (спецификацию) в виде (для детали эта команда вызывает ошибку):" +
        //        "\nКомпонент\n  |\n  Узел\n  | |\n  | Деталь\n  |\n  Деталь\n\n");
        //    help.Append("Print (*) — выводит на экран построчно список компонентов в формате:\n" +
        //        "Наименование\tТип\n\n");
        //    help.Append("Help [имя файла] — выводит на экран или в указанный файл список команд.\n\n");
        //    help.Append("Exit — закрывает все файлы и завершает программу. Файлы при завершении программы не уничтожаются.\n");

        //    Console.Write(help.ToString());
        //}

        public void Input(string index, string value)
        {
            if (manager == null)
                throw new FileNotFoundException(fileNotFoundExc);

            if (index == null && value == null)
                throw new ArgumentNullException();

            int.TryParse(index, out int _index);

            if (CheckInt(value))
            {
                int.TryParse(value, out int _value);

                manager.AddValueToArray(_index, _value);
            }

            else if (CheckString(value))
            {
                string _value = value.Replace("\"", "");

                manager.AddValueToArray(_index, _value);
            }

            else if (CheckChar(value))
            {
                char _value = value[1];

                manager.AddValueToArray(_index, _value);
            }

            else
                throw new Exception("Не прошёл проверку!");
        }

        private static bool CheckInt(string value)
        {
            if (int.TryParse(value, out _))
            {
                return true;
            }
            return false;
        }

        private static bool CheckString(string value)
        {
            if (value.First() == '\"' && value.Last() == '\"') return true;
            return false;
        }

        private static bool CheckChar(string value)
        {
            if (value.First() == '\'' && value.Last() == '\''
                && value.Length == 3 && !int.TryParse(value, out _)) return true;
            return false;
        }

        public void Print(int index)
        {
            if (index == null)
                throw new ArgumentNullException();

            manager.Print(index);
        }
    }
}