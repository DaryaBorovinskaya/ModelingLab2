using ModelingLab2;


Console.BackgroundColor = ConsoleColor.White;
Console.Clear();
Console.ForegroundColor = ConsoleColor.DarkMagenta;

PerformanceIndicators performanceIndicators = new ();
int multiOfTime = 1; // 60 for minutes, 3600 for hours
Random random = new();
double randomNullOne;
int N = 0,
    Nwb1 = 0, Nwb2 = 0,
    Nserv = 0, N_noServ = 0;

double Tstagn1 = 0.0, Tstagn2 = 0.0, Tserv = 0.0, Tmod = 24.0 * multiOfTime,
    t_free1 = 0.0, t_free2 = 0.0, t_com1 = 0.0, t_com2 = 0.0,
    Twait1 = 0.0, Twait2 = 0.0, t_beginServ1 = 0.0, t_beginServ2 = 0.0,
    delta_t_serv = 0.0, t_waitInBuff = 0.0,
    coeffWorkload = 0.0, T_averServ = 0.0, P_noServ = 0.0;

Application[] buffer1 = new Application[4];
Application[] buffer2 = new Application[4];

bool block = false, inBuff = false, fromBuff = false, breaking = false;


//int j = 0;
//Tmod = 10.0;
while (true)
{ 
    randomNullOne = random.NextDouble();
    t_com1 += -0.4 * Math.Log2(1 - randomNullOne)*multiOfTime;  //1
    //j++;
    Console.WriteLine($"{N+1} заявка, время прихода: {t_com1}");

    if(!block || Nwb1<4)   //2
    {
        if (t_com1 >= Tmod)   //3
        {
            coeffWorkload = performanceIndicators.CalculateCoeffWorkload(Tstagn1, Tstagn2, Tmod);  //18
            T_averServ = performanceIndicators.CalculateT_averServ(Tserv, Twait1, Twait2, Nserv);
            P_noServ = performanceIndicators.CalculateP_noServ(N_noServ, N+1);
            break;
        }

        if(t_com1 < t_free1 || Nwb1 != 0)   //4
        {
            fromBuff = false;           //5
            inBuff = false;

            if(Nwb1 < 4)   //7
            {
                inBuff = true;     //8
                buffer1[Nwb1] = new Application(N+1, t_com1, t_free1-t_com1+1.25*Nwb1*multiOfTime);
                Nwb1++;
            }

            if (block)        //9
                continue;

            if (buffer1[0].TimeComing + buffer1[0].TimeWaitInBuff == t_free1 && t_com1 >= t_free1)   //10
            {
                t_beginServ1 = t_free1;    //11
                Twait1 += buffer1[0].TimeWaitInBuff;
                delta_t_serv = 1.25 * multiOfTime;
                t_free1 = t_beginServ1 + delta_t_serv;

                for(int i = 0; i < buffer1.Length-1; i++) 
                    buffer1[i] = buffer1[i+1];

                Nwb1--;
                fromBuff = true;
            }

            if(!inBuff) //12
            {
                if(Nwb1 >= 4 || t_com1<t_beginServ1)   //13
                {
                    N++;    //14
                    N_noServ++;
                    continue;
                }
                else
                {
                    //15
                    buffer1[Nwb1] = new Application(N + 1, t_com1, t_free1 - t_com1 + 1.25 * Nwb1 * multiOfTime);
                    Nwb1++;
                    inBuff = true;
                }
            }

            

        }
        else
        {
            t_beginServ1 = t_com1;    //6
            Tstagn1 += (t_com1 - t_free1);
            delta_t_serv = 1.25 * multiOfTime;
            t_free1 = t_beginServ1 + delta_t_serv;
            fromBuff = true;
        }


        if (t_free1 >= Tmod)  //16
        {
            coeffWorkload = performanceIndicators.CalculateCoeffWorkload(Tstagn1,Tstagn2,Tmod);  //18
            T_averServ = performanceIndicators.CalculateT_averServ(Tserv+delta_t_serv,Twait1, Twait2, Nserv);
            P_noServ = performanceIndicators.CalculateP_noServ(N_noServ,N+1);
            break;
        }

        if(!fromBuff)    //17
        {
            N++;    //19
            continue;
        }

        Tserv += delta_t_serv;   //20


        t_com2 = t_free1;   //21
        
    }

    fromBuff = false;

    do
    {
        if (!block)  //22
        {
            if (t_com2 >= Tmod)   //23
            {
                coeffWorkload = performanceIndicators.CalculateCoeffWorkload(Tstagn1, Tstagn2, Tmod);  //18
                T_averServ = performanceIndicators.CalculateT_averServ(Tserv, Twait1, Twait2, Nserv);
                P_noServ = performanceIndicators.CalculateP_noServ(N_noServ, N + 1);
                breaking = true;
                break;
            }

            if (!(t_com2 < t_free2 || Nwb2 != 0))  //24
            {
                t_beginServ2 = t_com2;   //26
                Tstagn2 += (t_com2 - t_free2);
                delta_t_serv = 0.5 * multiOfTime;
                t_free2 = t_beginServ2 + delta_t_serv;
                fromBuff = true;
                inBuff = true;
                break;
            }

        }

        //25
        inBuff = false;
        fromBuff = false;

        if (Nwb2 < 2)   //27
        {
            inBuff = true;   //28
            buffer2[Nwb2] = new Application(N + 1, t_com2, t_free2 - t_com2 + 0.5 * Nwb2 * multiOfTime);
            Nwb2++;
        }

        if (buffer2[0].TimeComing + buffer2[0].TimeWaitInBuff == t_free2 && t_com2 >= t_free2)  //29
        {
            t_beginServ2 = t_free2; //30
            Twait2 += buffer2[0].TimeWaitInBuff;
            delta_t_serv = 0.5 * multiOfTime;
            t_free2 = t_beginServ2 + delta_t_serv;

            buffer2[0] = buffer2[1];
            Nwb2--;
            fromBuff = true;

            if (block)  //31
                block = false;  //32
        }
        

    } while (block);

    if (breaking)
        break;

    if (!inBuff)  //33
    {
        if (Nwb2 >= 2 || t_com2 < t_beginServ2) //34
        {
            block = true;    //35
            continue;
        }
        else
        {
            //36
            buffer2[Nwb2] = new Application(N + 1, t_com2, t_free2 - t_com2 + 0.5 * Nwb2 * multiOfTime);
            Nwb2++;
            inBuff = true;
        }
    }


    if(t_free2>=Tmod)   //37
    {
        //18
        coeffWorkload = performanceIndicators.CalculateCoeffWorkload(Tstagn1, Tstagn2, Tmod);  //18
        T_averServ = performanceIndicators.CalculateT_averServ(Tserv+delta_t_serv, Twait1, Twait2, Nserv);
        P_noServ = performanceIndicators.CalculateP_noServ(N_noServ, N+1);
        break;
    }
    
    if(!fromBuff)  //38
    {
        N++;  //39
    }
    else
    {
        Tserv += delta_t_serv;  //40
        Nserv++;
        N++;
    }
    
}

Console.WriteLine("\nРасчёт показателей эффективности функционирования системы:");
Console.WriteLine($"\nКоэффициент загруженности: {coeffWorkload}");
Console.WriteLine($"Среднее время обслуживания: {T_averServ}");
Console.WriteLine($"Вероятность попадания в отложенные: {P_noServ}");