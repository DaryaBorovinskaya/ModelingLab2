namespace ModelingLab2
{
    public class ModelingProcess
    {
        private PerformanceIndicators _performanceIndicators = new();
        private Random _random = new();
        public (decimal coeffWorkload, decimal T_averServ, decimal P_noServ) StartProcess()
        {
            decimal randomNullOne;
            int N = 0,
                Nwb1 = 0, Nwb2 = 0,
                Nserv = 0, N_noServ = 0;

            decimal Tstagn1 = 0.0m, Tstagn2 = 0.0m, Tserv = 0.0m, Tmod = 500.0m,
                t_free1 = 0.0m, t_free2 = 0.0m, t_com1 = 0.0m, t_com2 = 0.0m,
                Twait_serv1 = 0.0m, Twait_serv2 = 0.0m, t_beginServ1 = 0.0m, t_beginServ2 = 0.0m,
                delta_t_serv1 = 1.25m, delta_t_serv2 = 0.5m, timeWaitInBuff1 = 0.0m,
                coeffWorkload = 0.0m, T_averServ = 0.0m, P_noServ = 0.0m;

            int sizeBuff1 = 2;

            Application[] buffer1 = new Application[sizeBuff1];
            Application[] buffer2 = new Application[2];

            bool block = false, inBuff = false, fromBuff = false, breaking = false;


            //int j = 0;
            //Tmod = 4.4m;
            while (true)
            {
                randomNullOne = (decimal)_random.NextDouble();
                t_com1 += -0.4m * (decimal)Math.Log2((double)(1 - randomNullOne));  //1   //0.4m; 
                //j++;
                Console.WriteLine($"{N + 1} заявка, время прихода: {t_com1}");

                if (!block || Nwb1 < sizeBuff1)   //2
                {
                    if (t_com1 >= Tmod)   //3
                    {
                        coeffWorkload = _performanceIndicators.CalculateCoeffWorkload(Tstagn1, Tstagn2, Tmod);  //18
                        T_averServ = _performanceIndicators.CalculateT_averServ(Tserv, Twait_serv1, Twait_serv2, Nserv);
                        P_noServ = _performanceIndicators.CalculateP_noServ(N_noServ, N + 1);
                        break;
                    }

                    if (t_com1 < t_free1 || Nwb1 != 0)   //4
                    {
                        fromBuff = false;           //5
                        inBuff = false;

                        if (Nwb1 < sizeBuff1)   //7
                        {
                            inBuff = true;     //8
                            buffer1[Nwb1] = new Application(N + 1, t_com1, t_free1 - t_com1 + delta_t_serv1 * Nwb1);
                            Nwb1++;
                        }

                        if (block)        //9
                            continue;

                        if (buffer1[0].TimeComing + buffer1[0].TimeWaitInBuff == t_free1 && t_com1 >= t_free1)   //10
                        {
                            if (t_free1 + delta_t_serv1 > Tmod)
                            {
                                coeffWorkload = _performanceIndicators.CalculateCoeffWorkload(Tstagn1, Tstagn2, Tmod);  //18
                                T_averServ = _performanceIndicators.CalculateT_averServ(Tserv, Twait_serv1, Twait_serv2, Nserv);
                                P_noServ = _performanceIndicators.CalculateP_noServ(N_noServ, N + 1);
                                break;
                            }

                            t_beginServ1 = t_free1;    //11
                            timeWaitInBuff1 = buffer1[0].TimeWaitInBuff;
                            t_free1 = t_beginServ1 + delta_t_serv1;

                            for (int i = 0; i < buffer1.Length - 1; i++)
                                buffer1[i] = buffer1[i + 1];

                            Nwb1--;
                            fromBuff = true;
                        }

                        if (!inBuff) //12
                        {
                            if (Nwb1 >= sizeBuff1 || t_com1 < t_beginServ1)   //13
                            {
                                N++;    //14
                                N_noServ++;
                                continue;
                            }
                            else
                            {
                                //15
                                buffer1[Nwb1] = new Application(N + 1, t_com1, t_free1 - t_com1 + delta_t_serv1 * Nwb1);
                                Nwb1++;
                                inBuff = true;
                            }
                        }



                    }
                    else
                    {
                        if (t_com1 + delta_t_serv1 > Tmod)
                        {
                            Tstagn1 += (t_com1 - t_free1);
                            coeffWorkload = _performanceIndicators.CalculateCoeffWorkload(Tstagn1, Tstagn2, Tmod);  //18
                            T_averServ = _performanceIndicators.CalculateT_averServ(Tserv, Twait_serv1, Twait_serv2, Nserv);
                            P_noServ = _performanceIndicators.CalculateP_noServ(N_noServ, N + 1);
                            break;
                        }
                        t_beginServ1 = t_com1;    //6
                        Tstagn1 += (t_com1 - t_free1);
                        t_free1 = t_beginServ1 + delta_t_serv1;
                        fromBuff = true;
                    }


                    if (t_free1 >= Tmod)  //16
                    {
                        coeffWorkload = _performanceIndicators.CalculateCoeffWorkload(Tstagn1, Tstagn2, Tmod);  //18
                        T_averServ = _performanceIndicators.CalculateT_averServ(Tserv, Twait_serv1, Twait_serv2, Nserv);
                        P_noServ = _performanceIndicators.CalculateP_noServ(N_noServ, N + 1);
                        break;
                    }

                    if (!fromBuff)    //17
                    {
                        N++;    //19
                        continue;
                    }

                    //Tserv += delta_t_serv;   //20


                    t_com2 = t_free1;   //21

                }

                fromBuff = false;

                do
                {
                    if (!block)  //22
                    {
                        if (t_com2 >= Tmod)   //23
                        {
                            coeffWorkload = _performanceIndicators.CalculateCoeffWorkload(Tstagn1, Tstagn2, Tmod);  //18
                            T_averServ = _performanceIndicators.CalculateT_averServ(Tserv, Twait_serv1, Twait_serv2, Nserv);
                            P_noServ = _performanceIndicators.CalculateP_noServ(N_noServ, N + 1);
                            breaking = true;
                            break;
                        }

                        if (!(t_com2 < t_free2 || Nwb2 != 0))  //24
                        {
                            if(t_com2 + delta_t_serv2 > Tmod)
                            {
                                Tstagn2 += (t_com2 - t_free2);
                                coeffWorkload = _performanceIndicators.CalculateCoeffWorkload(Tstagn1, Tstagn2, Tmod);  //18
                                T_averServ = _performanceIndicators.CalculateT_averServ(Tserv, Twait_serv1, Twait_serv2, Nserv);
                                P_noServ = _performanceIndicators.CalculateP_noServ(N_noServ, N + 1);
                                breaking = true;
                                break;
                            }
                            t_beginServ2 = t_com2;   //26
                            Twait_serv1 += timeWaitInBuff1;
                            Tstagn2 += (t_com2 - t_free2);
                            t_free2 = t_beginServ2 + delta_t_serv2;
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
                        buffer2[Nwb2] = new Application(N + 1, t_com2, t_free2 - t_com2 + delta_t_serv2 * Nwb2);
                        Nwb2++;
                    }

                    if (buffer2[0].TimeComing + buffer2[0].TimeWaitInBuff == t_free2 && t_com2 >= t_free2)  //29
                    {
                        if (t_free2 + delta_t_serv2 > Tmod)
                        {
                            coeffWorkload = _performanceIndicators.CalculateCoeffWorkload(Tstagn1, Tstagn2, Tmod);  //18
                            T_averServ = _performanceIndicators.CalculateT_averServ(Tserv, Twait_serv1, Twait_serv2, Nserv);
                            P_noServ = _performanceIndicators.CalculateP_noServ(N_noServ, N + 1);
                            breaking = true;
                            break;
                        }

                        t_beginServ2 = t_free2; //30
                        Twait_serv1 += timeWaitInBuff1;
                        Twait_serv2 += buffer2[0].TimeWaitInBuff;
                        t_free2 = t_beginServ2 + delta_t_serv2;

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
                        buffer2[Nwb2] = new Application(N + 1, t_com2, t_free2 - t_com2 + delta_t_serv2 * Nwb2);
                        Nwb2++;
                        inBuff = true;
                    }
                }


                if (t_free2 >= Tmod)   //37
                {
                    //18
                    coeffWorkload = _performanceIndicators.CalculateCoeffWorkload(Tstagn1, Tstagn2, Tmod);  //18
                    T_averServ = _performanceIndicators.CalculateT_averServ(Tserv + delta_t_serv1+delta_t_serv2, Twait_serv1, Twait_serv2, Nserv);
                    P_noServ = _performanceIndicators.CalculateP_noServ(N_noServ, N + 1);
                    break;
                }

                if (!fromBuff)  //38
                {
                    N++;  //39
                }
                else
                {
                    Tserv += delta_t_serv1 + delta_t_serv2;  //40
                    Nserv++;
                    N++;
                }

            }

            return (coeffWorkload,T_averServ,P_noServ);
        }
    }
}

