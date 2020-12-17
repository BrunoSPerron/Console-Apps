using System;
using System.Text;
using System.ComponentModel;    //Needed to throw win32exception
using System.Runtime.InteropServices;   //Needed for Dll Import
using Microsoft.Win32.SafeHandles;  //Needed to gain acess to the visual console buffer

namespace TacticRoguelikeRpg
{
    class Helper
    {
        static CHAR_INFO[][,] ConsoleClipboard = new CHAR_INFO[2][,];

        public static int ShowMenuAndGetChoice(string[] _options, int _xOrigin = -1, int _yOrigin = -1, int _xMenuSize = 10, int _startingPosition = 1, bool[] _disabledOptions = null, bool UserCanCancel = true)
        {
            //Make sure entries values won't cause error
            if (_xOrigin < 0 || _xOrigin + _xMenuSize > Console.WindowWidth)
                _xOrigin = Console.CursorLeft;
            if (_yOrigin < 0 || _yOrigin > Console.WindowHeight)
                _yOrigin = Console.CursorTop;
            if (_disabledOptions == null)
                _disabledOptions = new bool[_options.Length];
            if (_startingPosition < 1 || _startingPosition > _options.Length)
                _startingPosition = 1;

            Console.ForegroundColor = Settings.DefaultForegroundColor;

            //Catch errors
            if (_disabledOptions.Length != _options.Length)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine();
                Console.WriteLine("ERROR(helper): PASSED UNMATCHING ARRAYS SIZE TO Helper.ShowMenuAndGetChoice()");
                Console.ReadKey();
                Console.WriteLine("Initializing default _disabledOptions[]");
                Console.ReadKey();
                _disabledOptions = new bool[_options.Length];
            }

            if (_options.Length == 0)
                return -1;

            //Normalize options length and Write each on Screen
            for (int i = 0; i < _options.Length; i++)
            {
                while (_options[i].Length < _xMenuSize)
                    _options[i] += " ";

                if (_disabledOptions[i])
                    Console.ForegroundColor = Settings.DisabledColor;
                if (i + 1 == _startingPosition)
                    Console.BackgroundColor = Settings.DefaultSelectionColor;

                Console.SetCursorPosition(_xOrigin, _yOrigin + i);
                Console.Write(_options[i].Substring(0, _xMenuSize));

                Console.ForegroundColor = Settings.DefaultForegroundColor;
                Console.BackgroundColor = Settings.DefaultBackgroundColor;
            }

            //Selection process
            int choice = _startingPosition;
            ConsoleKeyInfo userInput;
            bool hasFinished = false;
            while (!hasFinished)
            {
                userInput = Helper.GetUserInput(new ConsoleKey[] { ConsoleKey.UpArrow, ConsoleKey.NumPad8, ConsoleKey.DownArrow, ConsoleKey.NumPad2 }, true, true);

                switch (userInput.Key)
                {
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.NumPad8:
                        if (_options.Length == 1)
                            continue;

                        if (choice == 1)
                        {
                            WriteStringAtPosition(_options[choice - 1], _xOrigin, _yOrigin + choice - 1, false);
                            choice = _options.Length;
                            WriteStringAtPosition(_options[choice - 1], _xOrigin, _yOrigin + choice - 1, true);
                        }
                        else
                        {
                            WriteStringAtPosition(_options[choice - 1], _xOrigin, _yOrigin + choice - 1, false);
                            choice--;
                            WriteStringAtPosition(_options[choice - 1], _xOrigin, _yOrigin + choice - 1, true);
                        }

                        break;
                    case ConsoleKey.DownArrow:
                    case ConsoleKey.NumPad2:

                        if (_options.Length == 1)
                            continue;

                        if (choice == _options.Length)
                        {
                            WriteStringAtPosition(_options[choice - 1], _xOrigin, _yOrigin + choice - 1, false);
                            choice = 1;
                            WriteStringAtPosition(_options[choice - 1], _xOrigin, _yOrigin + choice - 1, true);
                        }
                        else
                        {
                            WriteStringAtPosition(_options[choice - 1], _xOrigin, _yOrigin + choice - 1, false);
                            choice++;
                            WriteStringAtPosition(_options[choice - 1], _xOrigin, _yOrigin + choice - 1, true);

                        }

                        break;
                    case ConsoleKey.Spacebar:
                    case ConsoleKey.Enter:
                    case ConsoleKey.NumPad5:
                        if (!_disabledOptions[choice - 1])
                            hasFinished = true;
                        break;
                    case ConsoleKey.Escape:
                    case ConsoleKey.Backspace:
                        choice = -1;
                        hasFinished = true;
                        break;
                }
            }

            Console.BackgroundColor = Settings.DefaultBackgroundColor;
            return choice;
        }

        static void WriteStringAtPosition(string toWrite, int _x, int _y, bool isSelected = false, bool isDisabled = false)
        {
            if (isSelected)
                Console.BackgroundColor = Settings.DefaultSelectionColor;
            if (isDisabled)
                Console.ForegroundColor = Settings.DisabledColor;
            Console.SetCursorPosition(_x, _y);
            Console.Write(toWrite);
            Console.ForegroundColor = Settings.DefaultForegroundColor;
            Console.BackgroundColor = Settings.DefaultBackgroundColor;
        }

        public static ConsoleKeyInfo GetUserInput(ConsoleKey[] validInput = null, bool addConfirmInput = false, bool addCancelInput = false, bool addLetterInput = false)
        {
            ConsoleKeyInfo userInput = new ConsoleKeyInfo();

            if (validInput == null)     //prevent error when passing null ConsoleKey array
                validInput = new ConsoleKey[0];

            if (addConfirmInput)
            {
                ConsoleKey[] _newValidInput = new ConsoleKey[validInput.Length + 3];

                for (int i = 0; i < validInput.Length; i++)
                    _newValidInput[i] = validInput[i];

                _newValidInput[validInput.Length] = ConsoleKey.Enter;
                _newValidInput[validInput.Length + 1] = ConsoleKey.Spacebar;
                _newValidInput[validInput.Length + 2] = ConsoleKey.NumPad5;
                validInput = _newValidInput;
            }

            if (addCancelInput)
            {
                ConsoleKey[] _newValidInput = new ConsoleKey[validInput.Length + 2];

                for (int i = 0; i < validInput.Length; i++)
                    _newValidInput[i] = validInput[i];

                _newValidInput[validInput.Length] = ConsoleKey.Escape;
                _newValidInput[validInput.Length + 1] = ConsoleKey.Backspace;
                validInput = _newValidInput;
            }

            // Clear Console Buffer (input entered during processing, or during System.Threading.Thread.Sleep())
            while (Console.KeyAvailable)
                Console.ReadKey(true);

            if (validInput == null)
            {
                Console.WriteLine("ERROR - Helper.GetUserInput() WAS CALLED WITHOUT VALID INPUTS");
                Console.ReadKey(true);
                Console.WriteLine("Enter a valid input now if you wish to continue");
                return Console.ReadKey(true);
            }

            bool hasFinished = false;
            while (!hasFinished)
            {
                userInput = Console.ReadKey(true);
                foreach (ConsoleKey _keyCode in validInput)
                {
                    if (userInput.Key == _keyCode || (char.IsLetter(userInput.KeyChar) && addLetterInput))
                        hasFinished = true;
                }
            }
            return userInput;
        }

        public static string ReadAtPosition(int _x, int _y, int _maxChar = 30)
        {
            String newLine = "";

            bool cursorVisibilityState = Console.CursorVisible;

            string eraser = "";
            for (int i = 0; i < _maxChar; i++)
                eraser += " ";

            Console.SetCursorPosition(_x, _y);
            Console.Write(eraser);
            Console.SetCursorPosition(_x, _y);
            Console.CursorVisible = true;

            //had to write a limited version of Console.ReadLine() there
            bool confirm = false;
            while (!confirm)
            {
                ConsoleKeyInfo input = GetUserInput(null, true, true, true);

                switch (input.Key)
                {
                    case ConsoleKey.Enter:
                    case ConsoleKey.NumPad5:
                        confirm = true;
                        break;
                    case ConsoleKey.Escape:
                        newLine = "";
                        confirm = true;
                        break;
                    case ConsoleKey.Backspace:
                        if (newLine.Length > 0)
                        {
                            newLine = newLine.Substring(0, newLine.Length - 1);
                            Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                            Console.Write(" ");
                            Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                            Console.CursorVisible = true;
                        }
                        break;
                    case ConsoleKey.NumPad0:
                    case ConsoleKey.NumPad1:
                    case ConsoleKey.NumPad2:
                    case ConsoleKey.NumPad3:
                    case ConsoleKey.NumPad4:
                    case ConsoleKey.NumPad6:
                    case ConsoleKey.NumPad7:
                    case ConsoleKey.NumPad8:
                    case ConsoleKey.NumPad9:
                        break;
                    case ConsoleKey.Spacebar:
                        if (newLine.Length < _maxChar)
                        {
                            Console.Write(" ");
                            newLine += (" ");
                        }
                        break;
                    default:
                        if (newLine.Length < _maxChar)
                        {
                            bool isUpper = false;
                            bool shift = false;
                            bool CapsLock = (((ushort)GetKeyState(0x14)) & 0xffff) != 0;
                            if ((input.Modifiers & ConsoleModifiers.Shift) != 0)
                                shift = true;

                            if (shift && !CapsLock)
                                isUpper = true;
                            if (!shift && CapsLock)
                                isUpper = true;

                            if (isUpper)
                            {
                                Console.Write(input.KeyChar);
                                newLine += input.KeyChar;
                            }
                            else
                            {
                                Console.Write(input.KeyChar.ToString().ToLower());
                                newLine += input.KeyChar.ToString().ToLower();
                            }

                            if (newLine.Length == _maxChar)
                                Console.CursorVisible = false;
                        }
                        break;
                }
            }
            Console.CursorVisible = cursorVisibilityState;
            Console.SetCursorPosition(_x, _y);
            return newLine;
        }

        public static int SingleLineSelection(String[] options, int _x, int _y, int _length, int startingSelection = 0)
        {
            if (options.Length == 0)
                return -1;

            int currentSelection = startingSelection;

            bool hasChoosen = false;
            while (!hasChoosen)
            {
                string eraser = "";
                while (eraser.Length < _length)
                    eraser += " ";

                Console.SetCursorPosition(_x, _y);
                Console.Write(eraser);
                Console.SetCursorPosition(_x, _y);
                Console.Write("<< " + options[currentSelection]);
                Console.SetCursorPosition(_x + _length - 2, _y);
                Console.Write(">>");

                ConsoleKeyInfo input = GetUserInput(new ConsoleKey[] { ConsoleKey.LeftArrow, ConsoleKey.RightArrow, ConsoleKey.NumPad4, ConsoleKey.NumPad6 }, true, true);

                switch (input.Key)
                {
                    case ConsoleKey.LeftArrow:
                    case ConsoleKey.NumPad4:
                        if (currentSelection > 0)
                            currentSelection--;
                        else
                            currentSelection = options.Length - 1;
                        break;
                    case ConsoleKey.RightArrow:
                    case ConsoleKey.NumPad6:
                        if (currentSelection < options.Length - 1)
                            currentSelection++;
                        else
                            currentSelection = 0;
                        break;
                    case ConsoleKey.Escape:
                    case ConsoleKey.Backspace:
                        currentSelection = -1;
                        hasChoosen = true;
                        break;
                    case ConsoleKey.Enter:
                    case ConsoleKey.Spacebar:
                    case ConsoleKey.NumPad5:
                        hasChoosen = true;
                        break;
                }
            }

            return currentSelection;
        }

        public static int ExtendedBoxSelection(String[] _options, int _x, int _y, int _width, int _height, bool[] disabledoptions = null, bool canCancel = true)
        {
            if (disabledoptions == null)
            {
                bool[] newOptions = new bool[_options.Length];
            }

            if (_options.Length == 0)
                return -1;

            bool cursorVisibilityStatus = Console.CursorVisible;
            Console.CursorVisible = false;

            for (int i = 0; i < _options.Length; i++)
            {
                if (_options[i] == null)
                {
                    Console.WriteLine("");
                    Console.WriteLine("ERROR - Helper.ExtendedBoxSelection RECIEVE NULL OPTION");
                    Console.WriteLine("");
                    Console.ReadKey();
                }

                while (_options[i].Length < _width)
                    _options[i] += " ";
            }

            int scrolling = 0;
            int position = 1;

            string upLine = "^";
            while (upLine.Length < _width)
                upLine += "^";

            string downLine = "v";
            while (downLine.Length < _width)
                downLine += "v";

            fullLocalUpdate();

            ConsoleKeyInfo userInput;
            bool hasFinished = false;
            while (!hasFinished)
            {
                userInput = Helper.GetUserInput(new ConsoleKey[] { ConsoleKey.UpArrow, ConsoleKey.NumPad8, ConsoleKey.DownArrow, ConsoleKey.NumPad2 }, true, canCancel);
                switch (userInput.Key)
                {
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.NumPad8:
                        if (position == 1)
                        {
                            if (scrolling > 0)
                            {
                                scrolling--;
                                fullLocalUpdate();
                            }
                        }
                        else
                        {
                            WriteStringAtPosition(_options[scrolling + position - 1].Substring(0, _width), _x, _y + position, false, disabledoptions[position - 1]);
                            position--;
                            WriteStringAtPosition(_options[scrolling + position - 1].Substring(0, _width), _x, _y + position, true, disabledoptions[position - 1]);

                        }
                        break;
                    case ConsoleKey.DownArrow:
                    case ConsoleKey.NumPad2:
                        if (position == _height - 2)
                        {
                            if (scrolling + _height - 2 < _options.Length)
                            {
                                scrolling++;
                                fullLocalUpdate();
                            }
                        }
                        else
                        {
                            if (position < _options.Length && _options.Length > 1)
                            {
                                WriteStringAtPosition(_options[scrolling + position - 1].Substring(0, _width), _x, _y + position, false, disabledoptions[position - 1]);
                                position++;
                                WriteStringAtPosition(_options[scrolling + position - 1].Substring(0, _width), _x, _y + position, true, disabledoptions[position - 1]);
                            }
                        }
                        break;
                    case ConsoleKey.Enter:
                    case ConsoleKey.Spacebar:
                    case ConsoleKey.NumPad5:
                        if (!disabledoptions[position - 1])
                            hasFinished = true;
                        break;
                    case ConsoleKey.Backspace:
                    case ConsoleKey.Escape:
                        position = -1;
                        hasFinished = true;
                        break;
                    default:
                        break;
                }
            }

            Console.CursorVisible = cursorVisibilityStatus;
            return position;

            void fullLocalUpdate()
            {

                for (int i = 0; i <= _height - 2; i++)
                {
                    Console.BackgroundColor = Settings.DefaultBackgroundColor;

                    if (scrolling < 1)
                        Console.ForegroundColor = Settings.DisabledColor;
                    Console.SetCursorPosition(_x, _y);
                    Console.Write(upLine);
                    Console.ForegroundColor = Settings.DefaultForegroundColor;

                    if (i + 1 == position)
                        Console.BackgroundColor = Settings.DefaultSelectionColor;

                    Console.SetCursorPosition(_x, _y + i + 1);

                    if (i >= _height - 2)
                    {
                        if (scrolling + _height - 2 >= _options.Length)
                            Console.ForegroundColor = Settings.DisabledColor;

                        Console.Write(downLine);
                        Console.ForegroundColor = Settings.DefaultForegroundColor;
                    }
                    else
                    {
                        if (disabledoptions.Length > i + scrolling)
                        {
                            if (disabledoptions[i + scrolling])
                                Console.ForegroundColor = Settings.DisabledColor;
                            if (i < _options.Length)
                                Console.Write(_options[i + scrolling].Substring(0, _width));
                        }

                    }
                }
            }
        }

        public static void AnimatedMenuBoxOpening(int _x, int _y, int _x2, int _y2, string[] image = null, int _openingSpeed = 1000)
        {
            //cut and fill the image to fit the dimension, if needed
            if (image != null)
            {
                if (image.Length < _y2 - _y)
                {
                    string[] _resizedImage = new string[_y2 - _y];
                    for (int i = 0; i < _resizedImage.Length; i++)
                    {
                        if (image.Length > i)
                            _resizedImage[i] = image[i];
                        else
                            _resizedImage[i] = "";
                    }
                    image = _resizedImage;

                    for (int i = 0; i < image.Length; i++)
                    {
                        if (image[i].Length > _x2 - _x)
                            image[i] = image[i].Substring(0, _x2 - _x);
                        else while (image[i].Length <= (_x2 - _x))
                                image[i] += " ";
                    }
                }
            }

            //Grows a line from the center
            Console.ForegroundColor = Settings.UILineColor;
            Console.BackgroundColor = Settings.DefaultBackgroundColor;
            for (int i = 0; i < (_x2 - _x) / 2; i++)
            {
                string _toDraw = "╠";
                for (int j = 0; j < i; j++)
                    _toDraw += "══";
                _toDraw += "╣";
                Console.SetCursorPosition(_x + (_x2 - _x) / 2 - i, _y + (_y2 - _y) / 2);
                Console.Write(_toDraw);
                System.Threading.Thread.Sleep((int)(_openingSpeed / (_x2 - _x) / 2 * Settings.MenuOpeningSpeedMult));
            }

            System.Threading.Thread.Sleep((int)(_openingSpeed / 60 * Settings.MenuOpeningSpeedMult));

            //open it vertically
            for (int i = 0; i < (_y2 - _y) / 2; i++)
            {
                if (image != null)
                {
                    Console.ForegroundColor = Settings.DisabledColor;
                    Console.SetCursorPosition(_x + 1, _y + (_y2 - _y) / 2 - i);
                    Console.Write(image[(_y2 - _y) / 2 - i - 1]);
                    Console.SetCursorPosition(_x + 1, _y + (_y2 - _y) / 2 + i);
                    Console.Write(image[(_y2 - _y) / 2 + i - 1]);
                    Console.ForegroundColor = Settings.DefaultForegroundColor;

                    DrawMenuBox(_x, _y - 1 + ((_y2 - _y) / 2) - i, _x2, _y + ((_y2 - _y) / 2) + i + 1, false);
                }
                else
                    DrawMenuBox(_x, _y - 1 + ((_y2 - _y) / 2) - i, _x2, _y + ((_y2 - _y) / 2) + i + 1, true);

                System.Threading.Thread.Sleep((int)(_openingSpeed / (_x2 - _x) / 2 * 3 * Settings.MenuOpeningSpeedMult));
            }

            //Draw a menubox over everything to ensure the final result is fully shown
            DrawMenuBox(_x, _y, _x2, _y2, image, false);
        }

        public static void AnimatedMenuBoxClosing(int left, int top, int redrawSavedZone = 0, int width = -1, int height = -1, int _closingSpeed = 1000)
        {
            if (width == -1 && redrawSavedZone > 0)
                width = ConsoleClipboard[redrawSavedZone].GetLength(0);
            if (height == -1 && redrawSavedZone > 0)
                height = ConsoleClipboard[redrawSavedZone].GetLength(1);
            if (width == -1 && redrawSavedZone <= 0 || height == -1 && redrawSavedZone <= 0)
            {
                Console.WriteLine("");
                Console.Write("ERROR - HELPER.AnimatedMenuBox() PARAMETERS WOULD RESULT IN A 0 OR NEGATIVE SIZED ZONE");
                return;
            }
            if ((width < 1 && redrawSavedZone <= 0) || (height < 1 && redrawSavedZone <= 0) || (left + width > Console.BufferWidth && redrawSavedZone <= 0) || (top + height > Console.BufferHeight && redrawSavedZone <= 0))
            {
                Console.WriteLine("");
                Console.WriteLine("ERROR - HELPER.AnimatedMenuBoxClosing() PARAMETERS WOULD RESULT IN OUT OF BOUND ERROR");
                Console.ReadKey();
                return;
            }

            //Prepare things needed to write directly on the buffer
            RECT _zone = new RECT();
            _zone.Position(left, top, left + width, top + height);
            COORD origin = new COORD();
            origin.Coord(left, top);
            COORD size = new COORD();
            size.Coord(_zone.Right, _zone.Bottom);

            CHAR_INFO[,] ConsoleClipboardWithOffset = new CHAR_INFO[size.X, size.Y];

            if (redrawSavedZone > 0)
            {
                for (int i = 0; i < ConsoleClipboardWithOffset.GetLength(0); i++)
                {
                    for (int j = 0; j < ConsoleClipboardWithOffset.GetLength(1); j++)
                    {
                        if (i < left || j < top)
                        {
                            ConsoleClipboardWithOffset[i, j] = new CHAR_INFO();
                        }
                        else
                            ConsoleClipboardWithOffset[i, j] = ConsoleClipboard[redrawSavedZone][i - left, j - top];
                    }
                }
            }

            CHAR_INFO[] buf = new CHAR_INFO[ConsoleClipboardWithOffset.GetLength(0) * ConsoleClipboardWithOffset.GetLength(1)];
            int c = 0;
            for (int j = 0; j < size.Y; j++)
            {
                for (int i = 0; i < size.X; i++)
                {
                    buf[c] = ConsoleClipboardWithOffset[i, j];
                    c++;
                }
            }
            SafeFileHandle h = new SafeFileHandle(GetStdHandle(unchecked((uint)-11)), true);

            //Get CharInfo Color Values
            //byte color = (Settings.UILineColor.ToString() + Settings.DefaultBackgroundColor); //Check <<


            if (!WriteConsoleOutput(h, buf, size, origin, ref _zone))
                throw new Win32Exception(Marshal.GetLastWin32Error());
        }

        public static void DrawMenuBox(int _x, int _y, int _x2, int _y2, string[] image = null, bool _clearInside = true)
        {

            if (_clearInside)
                ClearZone((_x + 1), (_y + 1), (_x2 - _x - 1), (_y2 - _y - 1));

            //cut and fill the image to fit the dimension, if needed
            if (image != null)
            {
                for (int i = 0; i < image.Length; i++)
                {
                    if (image[i] == null)
                        continue;

                    if (image[i].Length > _x2 - _x)
                        image[i] = image[i].Substring(0, _x2 - _x);
                    else while (image[i].Length < _x2 - _x)
                            image[i] += " ";
                }
            }

            Console.ForegroundColor = Settings.UILineColor;

            Console.SetCursorPosition(_x, _y);
            string lineToWrite = "╔";
            while (lineToWrite.Length < _x2 - _x)
                lineToWrite += "═";
            lineToWrite += "╗";
            Console.Write(lineToWrite);

            for (int i = 1; i < _y2 - _y; i++)
            {
                Console.SetCursorPosition(_x, _y + i);
                Console.Write("║");
                if (image != null)
                {
                    Console.ForegroundColor = Settings.DisabledColor;
                    Console.SetCursorPosition(_x + 1, _y + i);
                    if (i > 0 && i <= image.Length)
                        Console.Write(image[i - 1]);
                    Console.ForegroundColor = Settings.UILineColor;
                }
                Console.SetCursorPosition(_x2, _y + i);
                Console.Write("║");
            }
            Console.SetCursorPosition(_x, _y2);
            lineToWrite = "╚";
            while (lineToWrite.Length < _x2 - _x)
                lineToWrite += "═";
            lineToWrite += "╝";
            Console.Write(lineToWrite);

        }
        public static void DrawMenuBox(int _x, int _y, int _x2, int _y2, bool _clearInside)
        {
            DrawMenuBox(_x, _y, _x2, _y2, null, _clearInside);
        }

        public static void ClearZone(int _x, int _y, int width, int height)
        {
            string eraser = "";
            while (eraser.Length < width)
                eraser += " ";

            for (int i = 0; i < height; i++)
            {
                Console.SetCursorPosition(_x, _y + i);
                Console.Write(eraser);
            }
        }

        public static void BackupZone(int _x, int _y, int width, int height, int position = 0)
        {
            RECT _zone = new RECT();
            _zone.Position(_x, _y, width + 1, height + 1);

            IntPtr buffer = Marshal.AllocHGlobal(Console.BufferWidth * Console.BufferHeight * Marshal.SizeOf(typeof(CHAR_INFO)));

            COORD coord = new COORD();
            COORD size = new COORD();
            size.X = (short)(_zone.Right - _zone.Left);
            size.Y = (short)(_zone.Bottom - _zone.Top);

            if (!ReadConsoleOutput(GetStdHandle(unchecked((uint)-11)), buffer, size, coord, ref _zone))
                throw new Win32Exception(Marshal.GetLastWin32Error());

            IntPtr ptr = buffer;

            CHAR_INFO[,] completeZoneInfo = new CHAR_INFO[size.X, size.Y];
            for (int i = 0; i < size.Y; i++)
            {
                for (int j = 0; j < size.X; j++)
                {
                    CHAR_INFO charInfo = (CHAR_INFO)Marshal.PtrToStructure(ptr, typeof(CHAR_INFO));
                    ptr += Marshal.SizeOf(typeof(CHAR_INFO));

                    completeZoneInfo[j, i] = charInfo;
                }
            }
            ConsoleClipboard[position] = completeZoneInfo;
        }

        public static void DrawZoneFromBackup(int left, int top, int width = -1, int height = -1, int position = 0)
        {
            if (width == -1)
                width = ConsoleClipboard[position].GetLength(0);
            if (height == -1)
                height = ConsoleClipboard[position].GetLength(1);

            if (width < 1 || height < 1 || left + width > Console.BufferWidth || top + height > Console.BufferHeight)
            {
                Console.WriteLine("");
                Console.WriteLine("ERROR - HELPER.DrawZoneFromBackup() parameters result in out of bound error");
                Console.ReadKey();
                return;
            }

            RECT _zone = new RECT();
            _zone.Position(left, top, width, height);

            COORD origin = new COORD();
            origin.Coord(left, top);
            COORD size = new COORD();
            size.Coord(_zone.Right, _zone.Bottom);

            CHAR_INFO[,] ConsoleClipboardWithOffset = new CHAR_INFO[left + width, top + height];
            for (int i = 0; i < ConsoleClipboardWithOffset.GetLength(0); i++)
            {
                for (int j = 0; j < ConsoleClipboardWithOffset.GetLength(1); j++)
                {
                    if (i < left || j < top)
                    {
                        ConsoleClipboardWithOffset[i, j] = new CHAR_INFO();
                    }
                    else
                        ConsoleClipboardWithOffset[i, j] = ConsoleClipboard[position][i - left, j - top];
                }
            }

            CHAR_INFO[] buf = new CHAR_INFO[ConsoleClipboardWithOffset.GetLength(0) * ConsoleClipboardWithOffset.GetLength(1)];
            int c = 0;
            for (int j = 0; j < size.Y; j++)
            {
                for (int i = 0; i < size.X; i++)
                {
                    buf[c] = ConsoleClipboardWithOffset[i, j];
                    c++;
                }
            }

            IntPtr buffer = GetStdHandle(unchecked((uint)-11));

            SafeFileHandle h = new SafeFileHandle(buffer, true);
            if (!WriteConsoleOutput(h, buf, size, origin, ref _zone))
                throw new Win32Exception(Marshal.GetLastWin32Error());

        }


        //Needed to get state of caps lock
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        public static extern short GetKeyState(int keyCode);

        //Needed to gain access to the Console Buffer
        [DllImport("Kernel32.dll", SetLastError = true)]
        static extern IntPtr GetStdHandle(uint nStdHandle);

        //Needed to write data in the Console Buffer
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool WriteConsoleOutput(
        SafeFileHandle hConsoleOutput,
        CHAR_INFO[] lpBuffer,   //Changed type to CHAR_INFO
        COORD dwBufferSize,
        COORD dwBufferCoord,
        ref RECT lpWriteRegion);

        //Needed to read data in the Console Buffer
        [DllImport("Kernel32", SetLastError = true)]
        static extern bool ReadConsoleOutput(
        IntPtr hConsoleOutput,
        IntPtr lpBuffer,
        COORD dwBufferSize,
        COORD dwBufferCoord,
        ref RECT lpWriteRegion);
    }


    //New structs "variable container" needed to read from the buffer. Rarely needed, unless you need to communicate with unmannaged code
    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public short Left;
        public short Top;
        public short Right;
        public short Bottom;

        public void Position(int _x, int _y, int width, int height)
        {
            Left = (short)_x;
            Top = (short)_y;
            Right = (short)(_x + width);
            Bottom = (short)(_y + height);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct COORD
    {
        public short X;
        public short Y;

        public void Coord(int _x, int _y)
        {
            X = (short)_x;
            Y = (short)_y;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct CHAR_INFO
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] charData;
        public short attributes;
    }
    
    public struct CYCLER
    {
        public int currentPosition { get; set; }
        public string[] options { get; set; }

        short posX;
        short posY;
        short width;

        public CYCLER(String[] _options, int _x = -1, int _y = -1, int _width = -1) : this()
        {
            int longestOption = 0;
            if (_width == -1)
            {
                foreach (string s in options)
                {
                    if (s.Length > longestOption)
                        longestOption = s.Length;
                }
                width = (short)longestOption;
            }
            else
            {
                width = (short)_width;
            }

            if (_x == -1 || _y == -1 || _x > Console.BufferWidth || _y > Console.BufferHeight)
            {
                posX = (short)Console.CursorLeft;
                posY = (short)Console.CursorTop;
            }
            else
            {
                posX = (short)_x;
                posY = (short)_y;
            }
            currentPosition = 0;
            UpdateOptions(_options);
        }

        void Next()
        {
            int _oldCursorPositionX = Console.CursorLeft;
            int _oldCursorPositionY = Console.CursorTop;

            Console.SetCursorPosition(posX, posY);

            if (currentPosition < options.Length)
                currentPosition++;
            else
                currentPosition = 0;

            Console.Write(options[currentPosition - 1]);

            Console.SetCursorPosition(_oldCursorPositionX, _oldCursorPositionY);
        }

        void UpdateOptions(String[] _options)
        {
            for (int i = 0; i < options.Length; i++)
                if (options[i].Length < width)
                    options[i] += " ";

            options = _options;

            int _oldCursorPositionX = Console.CursorLeft;
            int _oldCursorPositionY = Console.CursorTop;

            Console.SetCursorPosition(posX, posY);

            Console.Write(options[currentPosition - 1]);

            Console.SetCursorPosition(_oldCursorPositionX, _oldCursorPositionY);
        }
    }
}
