using System;
using System.Collections.Generic;
using System.Text;

namespace TacticRoguelikeRpg
{
    class MainMenu
    {
        public GameScreen Start()
        {
            Console.Clear();
            DrawBackground();
            DrawTitle();

            return OpenSelection();
        }

        private void DrawBackground()
        {
            ExtendedConsole.SetActiveLayer(0);
            string[] _image = new string[38];

            Console.ForegroundColor = Settings.DisabledColor;
            Console.BackgroundColor = Settings.DefaultBackgroundColor;
            _image[0] = ("yyyyyyyyyssssssssssssooooooooo+++++++++////////////:::::::::::::::::::::::::///////////////++++++++++++++ooooooooooooo");
            _image[1] = ("yyyyyyyssssssssssssooooooooo++++++++/////////::::::::::::::::::::::::::::::::::::://///////////+++++++++++++++oooooooo");
            _image[2] = ("yysssssssssssssoooooooooo+++++++////////:::::::::--------------------------::::::::::::://///////////+++++++++++++++++");
            _image[3] = ("sssssssssssoooooooooo++++++++////////::::---:/:-::///+++:-----/++++:...------::::---::::::::::///////////////+++/+++++");
            _image[4] = ("ssssssssoooooooooo++++++++///////:::::--:yhdmNNmNNMMNNNMMmmddNMMMMNmyoso:..:/oyhyo+/:--:::::::::::://///////////////++");
            _image[5] = ("sssoooooooooooo+++++++////////:::::---+hmMNMMNNNNNmmNNNMMNMMMMMMMMMMMMMMdmdmNNNNNNmdhy/----:::::::::::::::::://///////");
            _image[6] = ("ooooooooooo++++++++////////::::::---:sNNmmdmmNNMMMNhhNNNddNNMNNmmMMMMMMMMMMMMMdmmmddNNh+:-------:::::::::::::::://////");
            _image[7] = ("oooooo++++++++++///////:::::::-----odmdhdNNMNMmNdh:+ymyhhmdmNNNNNMNNMMMMMMMMMNhdmmhmddsho-.-------------::::::::::::::");
            _image[8] = ("++++++++++++///////::::::::--------ydymNMMmNmmdmdd+ydNNNhmNNMNNMNmNNMMMNmNmNNNNNMMNmmdyhys:....---------------::::::::");
            _image[9] = ("+++++++////////::::::::::-----./hdmNMMNMMMNdhddmdNyshdmmddhhmdmdNmNmNNddsyshmNmNNNNNNNNNmdh-........------------------");
            _image[10] = ("+++/////////:::::::::------.--+mMMMMMNMNMNNNNmmhhmmhymmmysyshmNydsyooy///hdyNmmshddmNNNNNmNs-.............------------");
            _image[11] = ("/////////:::::::::------..-/omNMMMNNNMNmNNmmNNmmhdNdhy+hmhohdds-ohy:sdmNNNMMMNMmNNmhymNNMdss+.`..................-----");
            _image[12] = ("/////::::::::::-------.-:shNMMMMMMNdmNmmmyshNdddmddmddyydyyhms+oommyyNNmdddmMNNMMNNNyhdNmNNss.```.....................");
            _image[13] = ("//::::::::::--------..-/hmmNNmNMMNmddNNmydNNmdmyyy/+mmMNs/:os`/s:/s//oyodsmdddmmdNmmdshdydmh/.```````.................");
            _image[14] = ("::::::::----------..-/shmMNNNNMMNNNhymhdNNhmdsdy+ohNNMMMms/.ds/sosmNhoyyyhmos+ydmhhNMdyhmhs++/o/..```````.............");
            _image[15] = ("::::::----------....:dmNNMNMMNNdhdNMNMNNmNmmhmhddNNNNmMmsmo/md++ydNMmyohysmsysdmmhymNNMMMMMNmmdhds:-`````````.........");
            _image[16] = ("::-----------......:smNMNmNNmNNNNMNNNMNNNNmmmmshNmdmmymy:--dh-./sdNsydyyhdsdoddmdmdNNNNNNMMMNNMNMNyyo-``````````......");
            _image[17] = ("-----------........-sNMNmdNdNNMNNmmmNNNmmdyhddNNNNNyhMMhs-/Ndyssds///soysdNmhdmhmhdmNNMNNNMMNmNNMNmmNy-```````````````");
            _image[18] = ("--------..........`-yNMNmNmmNNNmmNMddsddmdddNNdho+mmdmdNmmNy.oymmdmdyNNNmmNNNmhNmddNmmNmNMNNNmNNmdNmNMd:``       `````");
            _image[19] = ("------...........`./dMMmmNNmmNMNmNmddNNyNdhs+hd/--smNmmNMMh-`-+sNmshNNhhomdNNMNNmNNMNNNmhdyddhhdyhmmmMm/`       ``````");
            _image[20] = ("---..............`.-yNMmmNNdomNmmmmNNNNmdyhosdhmosymdsomMMo-.+ :NNs/+hm+oddhooyh/shmmmhhmyhydo+oooyymNdo.`          ``");
            _image[21] = ("-...............```.oNdhyso:/omhyhmd/-/oyoosdhyo++/:-`:NMMy.-oydMNhsos.:sdds:/osdyds+ydyssohodyyos+-yy:`              ");
            _image[22] = ("...............`````-//+oo/:+sdy+/+//+o/::-ssso////////hMMd`.hMMm/-+oo//:+s+oso+so+syysyssyosoyyo/-`                  ");
            _image[23] = ("..........```````````-..```.``--..-o+o+-:-.++/y+/:/--::/mMMdNMNs/-`` +osyo+/:+++/o++s+:-`.::::.`.```                  ");
            _image[24] = ("......`````````````````````      ` ` ``      .-... `.`.`sMN/hN+ `----:-`-://-::.```                                   ");
            _image[25] = (".......`````````````````````                            hMm.dN-                                                       ");
            _image[26] = ("...`````````````````````````                           /NMNyMN`                                              ```.``...");
            _image[27] = ("........`````````````````                            .yMMMMMMm.````````.......-:::::+/+//////////++////////++ssyshhhyh");
            _image[28] = ("/:-:/://:/+++///:///+ooooo+++/++++o+//+///:::::://:/:hMMMMMMMMNhosysyssssyysssssssssosssyyhhhhhhhhhhhdhhhyyhhddmddhmmm");
            _image[29] = ("yhdddhhhdddyyyyossyssssdddmdddddddmdhhdmdmmmdddhyhyyhhNMMMNNNMMmhs+ssyhysssyhhyyhhdddmddhdmmhhyhhdhhdmmmNNNmmNNNNNNNNN");
            _image[30] = ("ddddddddhyyssssyyhysoysddhsshyssyyysoo+sosyhhhdhdhhmmmmhysoosyyyysyshmmmdmddhoyyyhhhdmmmmmdmmmmNNmdhhyshmNNNmNNNNNNNNN");
            _image[31] = ("dhhyssssysysyhyyso+syddmhyshyyhhysssyhhdddddddmmmmmNmmdsoysyhyhyyhhydhmdhmmddhshhhydmdmNmmhyhdhddmNmNmmdddhddNNNNNmmmm");
            _image[32] = ("hyyhhhdhhdddhhhdddmNNNmmmNNNNmdhddmmNNNNNNNNNNNNmmmmmdssyhhssddhhshyhyo+oyhddddhhddmdhyhmmddhyhdddmddmNNNNNNNNNNNNNNNN");
            _image[33] = ("ddmmdmmmdmmdhdmmmNMMMmNNNNNNNNmdddmmNmNNNmNmmNNNNmmmmdsysdhyshyhhhhhsyooo+yhdddmdmmmmdhmmmmmdmmmddddydmNNNNNMNNNMNNNNM");
            _image[34] = ("dmNddmmmmdmmmNmNmNmNmmNNNNmmmNNmmmNmmmNNNNNmmmmmNmmmmmddhhhdhdydmmysyhyyhysyhmddhdmmmNmNmNNNNNMNNNNNNmNNNNNNNMMMMMMMNN");
            _image[35] = ("dddhhNdNNmNNNNNNdmmNmmmddmdmmmNmmNdmmmmmmdmmhmmNNmNmmmmmdddddysdmdoosddddddyyddmddmdmmNNmmNNNNNmmNNNNmmNNNNNNNMNmNNMMM");
            _image[36] = ("mddydddmNmMNNNmddmmmmmmdmmdmmNmddmmmdydhyddddydNNNNNmNNNmmdmdmhyyshhyhhddmmmmddNddhdmmmNmmNNNmmmNmmmNNdmmNmNmNMNNNMNMN");
            _image[37] = ("dNNNdmNmdNNmmddmmNmmmmmmmmdmmdmmmmhdysyhdmsydmNNNNNNNmmmmmNmdmdo++yyhdmmmmmmmddNNNNNmmmmNNNNMNmNNNNNNmmNNNNNmNNNNMNMNN");

            if (Settings.animatedMainMenu)
                ExtendedConsole.AnimatedMenuBoxOpening(0, 0, _image, 1000);
            else
            {
                ExtendedConsole.VirtualDrawBox(0, 0, 80, 36);
                ExtendedConsole.Update();
            }


            Console.ForegroundColor = Settings.DefaultForegroundColor;
        }

        private void DrawTitle()
        {
            ExtendedConsole.SetActiveLayer(1);
            string[] _image = new string[7];
            _image[0] = ("   ___  _     _            ___                         _       ");
            _image[1] = ("  |   \\(_)_ _(_)_ _  ___  | _ \\___ _ __  _ _  __ _ _ _| |_ ___ ");
            _image[2] = ("  | |) | \\ V / | ' \\/ -_) |   / -_) '  \\| ' \\/ _` | ' \\  _(_-<  ");
            _image[3] = ("  |___/|_|\\_/|_|_||_\\___| |_|_\\___|_|_|_|_||_\\__,_|_||_\\__/__/ ");
            _image[4] = ("                                                               ");
            _image[5] = ("                                                               ");
            _image[6] = ("                                                               ");

            if (Settings.animatedMainMenu)
                ExtendedConsole.AnimatedMenuBoxOpening(28, 5, _image, 800);
            else
            {
                ExtendedConsole.VirtualDrawBox(28, 5, 62, 4);
                ExtendedConsole.Update();
            }


            ExtendedConsole.SetActiveLayer(2);
            string subtitle = "   fragments of a shattered world   ";
            if (Settings.animatedMainMenu)
            {
                int charPositionIndex = 0;
                foreach (char _char in subtitle)
                {
                    ExtendedConsole.VirtualWrite(_char.ToString(), 33 + charPositionIndex, 11);
                    if (!Console.KeyAvailable)
                        System.Threading.Thread.Sleep(50);
                }
            }
            else
            {
                ExtendedConsole.VirtualWrite(subtitle, 33, 11);
                ExtendedConsole.Update();
            }
        }

        private GameScreen OpenSelection()
        {
            //Helper.AnimatedMenuBoxOpening(73, 11, 88, 15, null, 200);

            string[] possibleChoices = new string[] { " New Game ", " unimplemented ", " Exit " };
            ExtendedConsole.SetVirtualCursorPosition(74, 12);
            int choice = ExtendedConsole.ShowMenuAndGetChoice(possibleChoices);

            choice++; //temporary, remove when continue button is added

            switch (choice)
            {
                case 2:
                    break;
                case 3:
                    //settings
                    break;
                case 4:
                    Console.Clear();
                    Console.WriteLine("goodbye...");
                    System.Threading.Thread.Sleep(1000);
                    Environment.Exit(0);
                    break;
                default:
                    break;
            }
            return (GameScreen)choice;
        }
    }
}
