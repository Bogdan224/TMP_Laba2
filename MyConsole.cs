using System.Text;

namespace TMP_Laba2
{
    //public class MyConsole
    //{
    //    private const string startCommandLine = "VM>";
    //    private const string paramNotFoundExceptionText = "Не удалось найти подходящий параметр!";
    //    private const string paramNotExistsExceptionText = "У данной команды отсутствуют параметры!";
    //    private const string commandNotFoundExceptionText = "Команда не найдена!";

    //    public static void StartConsole()
    //    {
    //        string? commandLineText;
    //        ConsoleCommands commands = new ConsoleCommands();
    //        while (true)
    //        {
    //            Console.Write(startCommandLine);
    //            commandLineText = Console.ReadLine();

    //            if (commandLineText == null || commandLineText == "")
    //                continue;
    //            var commandText = commandLineText.Split();
    //            try
    //            {
    //                switch (commandText[0])
    //                {
    //                    //case "Create":
    //                    //    if (commandText.Length == 2)
    //                    //        commands.Create(commandText[1]);
    //                    //    else if (commandText.Length == 3)
    //                    //        commands.Create(commandText[1], Convert.ToUInt16(commandText[2]));
    //                    //    else if (commandText.Length == 4)
    //                    //        commands.Create(commandText[1], Convert.ToUInt16(commandText[2]), commandText[3]);
    //                    //    else
    //                    //        throw new ArgumentException(paramNotFoundExceptionText);
    //                    //    break;

    //                    //case "Open":
    //                    //    if (commandText.Length != 2)
    //                    //        throw new ArgumentException(paramNotFoundExceptionText);
    //                    //    commands.Open(commandText[1]);
    //                    //    break;

    //                    case "Input":
    //                        if (commandText.Length == 2 && commandText[1].Contains('/'))
    //                        {
    //                            var tmp = commandText[1].Split('/');
    //                            commands.Input(tmp[0], tmp[1]);
    //                        }
    //                        else if (commandText.Length == 3)
    //                            commands.Input(commandText[1], commandText[2]);
    //                        else
    //                            throw new ArgumentException(paramNotFoundExceptionText);
    //                        break;

    //                    case "Print":
    //                        if (commandText.Length != 2)
    //                            throw new ArgumentException(paramNotFoundExceptionText);

    //                        if (commandText[1] == "*")
    //                            commands.Print();
    //                        else
    //                            commands.Print(commandText[1]);
    //                        break;

    //                    case "Help":
    //                        if (commandText.Length > 2)
    //                            throw new ArgumentException(paramNotFoundExceptionText);
    //                        if (commandText.Length == 1)
    //                            commands.Help();
    //                        else if (commandText.Length == 2)
    //                            commands.Help(commandText[1]);
    //                        break;

    //                    case "Exit":
    //                        if (commandText.Length != 1)
    //                            throw new ArgumentException(paramNotExistsExceptionText);
    //                        commands.Exit();
    //                        return;

    //                    case "Test":
    //                        if (commandText.Length != 1)
    //                            throw new ArgumentException(paramNotExistsExceptionText);
    //                        commands.Test();
    //                        break;
    //                    default:
    //                        throw new ArgumentException(commandNotFoundExceptionText);

    //                }
    //            }
    //            catch (NotImplementedException)
    //            {
    //                Console.ForegroundColor = ConsoleColor.Red;
    //                Console.WriteLine("Команда разрабатывается!");
    //                Console.ResetColor();
    //            }
    //            catch (Exception e)
    //            {
    //                Console.ForegroundColor = ConsoleColor.Red;
    //                Console.WriteLine("Ошибка: " + e.Message);
    //                Console.ResetColor();
    //            }
    //        }
    //    }
    //}

    ///// <summary>
    ///// Команды для консоли
    ///// </summary>
    /////  : IDisposable
    //public class ConsoleCommands
    //{
    //    private FileManager? manager;
    //    private string path = @$"C:\Users\{Environment.UserName}\Downloads\";
    //    private const string fileNotFoundExc = "Для начала нужно создать или открыть файл!";

    //    private bool CheckFilename(string filename)
    //    {
    //        if (filename.EndsWith(".prd") && filename.Length <= 16)
    //            return true;
    //        return false;
    //    }

    //    ///// <summary>
    //    ///// Если файл существует и сигнатура соответствует заданию, команда требует
    //    ///// подтверждения на перезапись файла. При положительном ответе, файлы очищаются, после
    //    ///// чего создаются все необходимые структуры в памяти и файлах на диске. После успешного
    //    ///// выполнения команды файлы считаются открытыми для работы. Если сигнатура файла
    //    ///// отсутствует или не соответствует заданию, команда вызывает ошибку.
    //    ///// </summary>
    //    ///// <param name="filename">Имя файла</param>
    //    //public void Create(string filename, ushort recordLength = 20, string? specFilename = null)
    //    //{
    //    //    if (!CheckFilename(filename))
    //    //        throw new Exception("Нельзя создать файл с заданным расширением!");

    //    //    if (specFilename == null)
    //    //        specFilename = Path.ChangeExtension(filename, ".prs");

    //    //    if (manager != null)
    //    //        manager.Dispose();

    //    //    if (File.Exists(path + filename))
    //    //    {
    //    //        while (true)
    //    //        {
    //    //            Console.WriteLine("Перезаписать файлы? (Д/н)");
    //    //            string? ans = Console.ReadLine();
    //    //            if (ans == "Д")
    //    //            {
    //    //                manager = FileManager.RestoreFiles(filename, specFilename, recordLength);
    //    //                break;
    //    //            }
    //    //            else if (ans == "н")
    //    //            {
    //    //                manager = FileManager.OpenFiles(filename);
    //    //                break;
    //    //            }
    //    //            else
    //    //            {
    //    //                continue;
    //    //            }
    //    //        }
    //    //    }
    //    //    else
    //    //        manager = FileManager.CreateFiles(filename, specFilename, recordLength);
    //    //}

    //    /// <summary>
    //    /// Команда закрывает все файлы и завершает программу.
    //    /// </summary>
    //    public void Exit()
    //    {
    //        manager?.Dispose();
    //    }

    //    /// <summary>
    //    /// Команда выводит на экран или в указанный файл список команд.
    //    /// </summary>
    //    /// <param name="filename">Имя файла</param>
    //    public void Help(string? filename = null)
    //    {
    //        StringBuilder help = new StringBuilder();
    //        help.Append("Create (имя файла, [максимальная длина имени компонента], [имя файла спецификаций]) — если файл существует и сигнатура соответствует заданию, команда требует\n" +
    //            "подтверждения на перезапись файла. При положительном ответе, файлы очищаются, после чего создаются все необходимые структуры в памяти и файлах на диске.\n" +
    //            "После успешного выполнения команды файлы считаются открытыми для работы. Если сигнатура файла отсутствует или не соответствует заданию, команда вызывает ошибку.\n" +
    //            "Расширение имени файла для списка компонентов — «.prd»., а для файла спецификаций — «.prs».\n\n");
    //        help.Append("Open (имя файла) — открывает указанный файл и связанные с ним файлы в режиме rw, создает все необходимые структуры в памяти.\n" +
    //            "Если сигнатура файла отсутствует или несоответствует заданию, команда вызывает ошибку.\n\n");
    //        help.Append("Input (имя компонента, тип) — включает компонент в список. тип — одно из следующего: Изделие, Узел, Деталь.\n\n");
    //        help.Append("Input (имя компонента/имя комплектующего) — включает комплектующее в спецификацию компонента. Имя комплектующего должно быть в списке,\n" +
    //            "в противном случае и для детали эта команда вызывает ошибку.\n\n");
    //        help.Append("Print (имя компонента) — выводит на экран состав компонента (спецификацию) в виде (для детали эта команда вызывает ошибку):" +
    //            "\nКомпонент\n  |\n  Узел\n  | |\n  | Деталь\n  |\n  Деталь\n\n");
    //        help.Append("Print (*) — выводит на экран построчно список компонентов в формате:\n" +
    //            "Наименование\tТип\n\n");
    //        help.Append("Help [имя файла] — выводит на экран или в указанный файл список команд.\n\n");
    //        help.Append("Exit — закрывает все файлы и завершает программу. Файлы при завершении программы не уничтожаются.\n");

    //        Console.Write(help.ToString());
    //    }

    //    /// <summary>
    //    /// Команда включает значение в открытый массив.
    //    /// </summary>
    //    /// <param name="index">Индекс</param>
    //    /// <param name="value">Значение</param>
    //    public void Input(string index, string value)
    //    {
    //        if (manager == null)
    //            throw new FileNotFoundException(fileNotFoundExc);

    //        if (value.First() == '\"' && value.Last() == '\"')
    //        {
    //            //manager.AddValueToArray(new(index, value));
    //        }    
    //        if (va)

    //        Console.WriteLine("Значение добавлено!");
    //    }
    //    /// <summary>
    //    /// Команда включает комплектующее в
    //    /// спецификацию компонента. Имя комплектующего должно быть в списке, в противном
    //    /// случае и для детали эта команда вызывает ошибку.
    //    /// </summary>
    //    /// <param name="parentComponent">Имя компонента</param>
    //    /// <param name="componentAdded">Имя комплектующего</param>
    //    public void Input(string parentComponent, string componentAdded)
    //    {
    //        if (manager == null)
    //            throw new FileNotFoundException(fileNotFoundExc);
    //        manager.AddComponentToSpecification(parentComponent, componentAdded);
    //        Console.WriteLine("Компонент добавлен в спецификацию!");
    //    }

    //    /// <summary>
    //    /// Команда выводит на экран состав компонента (спецификацию) (для детали эта команда вызывает ошибку):
    //    /// </summary>
    //    /// <param name="componentName">Имя компонента</param>
    //    public void Print(string componentName)
    //    {
    //        if (manager == null)
    //            throw new FileNotFoundException(fileNotFoundExc);
    //        var graph = manager.GetCompWithSpecs(componentName);

    //        Console.WriteLine(graph.Value.ComponentName);

    //        var action = new Action<MyComponent, int>((comp, depth) =>
    //        {
    //            var str = "  |";
    //            var sb = new StringBuilder();

    //            // Первая строка
    //            sb.Append(string.Concat(Enumerable.Repeat(str, depth)));
    //            sb.AppendLine();

    //            // Вторая строка
    //            sb.Append(string.Concat(Enumerable.Repeat(str, depth - 1)));
    //            sb.AppendLine("  " + comp.ComponentName);

    //            Console.Write(sb.ToString());
    //        });

    //        graph.EnumerateComponents(action);
    //    }
    //    /// <summary>
    //    /// Команда выводит на экран построчно список компонентов.
    //    /// </summary>
    //    public void Print()
    //    {
    //        if (manager == null)
    //            throw new FileNotFoundException(fileNotFoundExc);

    //        var components = manager.GetAllComponents();

    //        if (components.Count() == 0)
    //        {
    //            Console.WriteLine("Список пустой!");
    //            return;
    //        }

    //        Console.WriteLine($"{"Наименование",-20}Тип");

    //        foreach (var component in components)
    //        {
    //            Console.WriteLine($"{component.ComponentName,-20}{component.ComponentType.ToStr()}");
    //        }
    //    }

    //    //public void Test()
    //    //{
    //    //    if (manager == null)
    //    //        throw new FileNotFoundException(fileNotFoundExc);
    //    //    manager.Test();
    //    //    Print("Изделие1");
    //    //}
    //}
}