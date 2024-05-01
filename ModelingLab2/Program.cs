using ModelingLab2;


Console.BackgroundColor = ConsoleColor.White;
Console.Clear();
Console.ForegroundColor = ConsoleColor.DarkMagenta;

ModelingProcess modelingProcess = new ();
StatisticalStability statistical = new();
(decimal coeffWorkload, decimal T_averServ, decimal P_noServ) result = modelingProcess.StartProcess();
//int countOfRealization = 50;

//decimal[] p = statistical.FindProbability(out countOfRealization);
//for (int i = 0; i < p.Length; i++)
//{
//    Console.WriteLine(p[i]);
//}

//Console.WriteLine($"\nВремя моделирования: 1000");
//Console.WriteLine($"Погрешность: 0,04");
//Console.WriteLine($"Необходимое число реализаций: {countOfRealization}");





Console.WriteLine("\nРасчёт показателей эффективности функционирования системы:");
Console.WriteLine($"\nКоэффициент загруженности: {result.coeffWorkload}");
Console.WriteLine($"Среднее время обслуживания: {result.T_averServ}");
Console.WriteLine($"Вероятность попадания в отложенные: {result.P_noServ}");