// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Keyboard.cs" company="KriaSoft LLC">
//   Copyright © 2013 Konstantin Tarkus, KriaSoft LLC. See LICENSE.txt
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;

namespace MilkyAmiBroker.Plugins.Models
{
    internal class Keyboard
    {
        public const int InputMouse = 0;
        public const int InputKeyboard = 1;
        public const int InputHardware = 2;
        public const uint KeyEventFExtendedKey = 0x0001;
        public const uint KeyEventFKeyUp = 0x0002;
        public const uint KeyEventFUnicode = 0x0004;
        public const uint KeyEventFScanCode = 0x0008;
        public const uint XButton1 = 0x0001;
        public const uint XButton2 = 0x0002;
        public const uint MouseEventFMove = 0x0001;
        public const uint MouseEventFLeftDown = 0x0002;
        public const uint MouseEventFLeftUp = 0x0004;
        public const uint MouseEventFRightDown = 0x0008;
        public const uint MouseEventFRightUp = 0x0010;
        public const uint MouseEventFMiddleDown = 0x0020;
        public const uint MouseEventFMiddleUp = 0x0040;
        public const uint MouseEventFXDown = 0x0080;
        public const uint MouseEventFXUp = 0x0100;
        public const uint MouseEventFWheel = 0x0800;
        public const uint MouseEventFVirtualDesk = 0x4000;
        public const uint MouseEventFAbsolute = 0x8000;

        [DllImport("user32.dll")]
        internal static extern IntPtr GetMessageExtraInfo();

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern uint SendInput(uint inputs, ref INPUT inputsPtr, int size);

        internal static void InsertSymbolsFromClipboard()
        {
            var input = new INPUT { Type = InputKeyboard, MouseKeyboardHardwareInput = new MOUSEKEYBDHARDWAREINPUT { KeyboardInput = new KEYBDINPUT { Scan = 0, Time = 0, Flags = 0, Vk = (ushort)VK.VK_MENU } } };
            SendInput(1, ref input, Marshal.SizeOf(input));

            input.MouseKeyboardHardwareInput.KeyboardInput.Vk = (ushort)0x53;
            SendInput(1, ref input, Marshal.SizeOf(input));

            input.MouseKeyboardHardwareInput.KeyboardInput.Flags = KeyEventFKeyUp;
            input.MouseKeyboardHardwareInput.KeyboardInput.Vk = (ushort)VK.VK_MENU;
            SendInput(1, ref input, Marshal.SizeOf(input));

            input.MouseKeyboardHardwareInput.KeyboardInput.Flags = KeyEventFKeyUp;
            input.MouseKeyboardHardwareInput.KeyboardInput.Vk = (ushort)0x53;
            SendInput(1, ref input, Marshal.SizeOf(input));

            input.MouseKeyboardHardwareInput.KeyboardInput.Flags = 0;
            input.MouseKeyboardHardwareInput.KeyboardInput.Vk = (ushort)0x4E;
            SendInput(1, ref input, Marshal.SizeOf(input));

            input.MouseKeyboardHardwareInput.KeyboardInput.Flags = KeyEventFKeyUp;
            input.MouseKeyboardHardwareInput.KeyboardInput.Vk = (ushort)0x4E;
            SendInput(1, ref input, Marshal.SizeOf(input));

            // CTR + P
            input.MouseKeyboardHardwareInput.KeyboardInput.Vk = (ushort)VK.VK_CONTROL;
            input.MouseKeyboardHardwareInput.KeyboardInput.Flags = 0;
            SendInput(1, ref input, Marshal.SizeOf(input));

            input.MouseKeyboardHardwareInput.KeyboardInput.Vk = (ushort)0x56;
            input.MouseKeyboardHardwareInput.KeyboardInput.Flags = 0;
            SendInput(1, ref input, Marshal.SizeOf(input));

            input.MouseKeyboardHardwareInput.KeyboardInput.Vk = (ushort)VK.VK_CONTROL;
            input.MouseKeyboardHardwareInput.KeyboardInput.Flags = KeyEventFKeyUp;
            SendInput(1, ref input, Marshal.SizeOf(input));

            input.MouseKeyboardHardwareInput.KeyboardInput.Vk = (ushort)0x56;
            input.MouseKeyboardHardwareInput.KeyboardInput.Flags = KeyEventFKeyUp;
            SendInput(1, ref input, Marshal.SizeOf(input));

            // Enter
            input.MouseKeyboardHardwareInput.KeyboardInput.Vk = (ushort)VK.VK_RETURN;
            input.MouseKeyboardHardwareInput.KeyboardInput.Flags = 0;
            SendInput(1, ref input, Marshal.SizeOf(input));

            input.MouseKeyboardHardwareInput.KeyboardInput.Vk = (ushort)VK.VK_RETURN;
            input.MouseKeyboardHardwareInput.KeyboardInput.Flags = KeyEventFKeyUp;
            SendInput(1, ref input, Marshal.SizeOf(input));

            // Enter
            input.MouseKeyboardHardwareInput.KeyboardInput.Vk = (ushort)VK.VK_RETURN;
            input.MouseKeyboardHardwareInput.KeyboardInput.Flags = 0;
            SendInput(1, ref input, Marshal.SizeOf(input));

            input.MouseKeyboardHardwareInput.KeyboardInput.Vk = (ushort)VK.VK_RETURN;
            input.MouseKeyboardHardwareInput.KeyboardInput.Flags = KeyEventFKeyUp;
            SendInput(1, ref input, Marshal.SizeOf(input));
        }

        internal struct MOUSEINPUT
        {
            public int Dx;
            public int Dy;
            public uint MouseData;
            public uint Flags;
            public uint Time;
            public IntPtr ExtraInfo;
        }

        internal struct KEYBDINPUT
        {
            public ushort Vk;
            public ushort Scan;
            public uint Flags;
            public uint Time;
            public IntPtr ExtraInfo;
        }

        internal struct HARDWAREINPUT
        {
            public int Message;
            public short ParamL;
            public short ParamH;
        }

        [StructLayout(LayoutKind.Explicit)]
        internal struct MOUSEKEYBDHARDWAREINPUT
        {
            [FieldOffset(0)]
            public MOUSEINPUT MouseInput;

            [FieldOffset(0)]
            public KEYBDINPUT KeyboardInput;

            [FieldOffset(0)]
            public HARDWAREINPUT HardwareInput;
        }

        internal struct INPUT
        {
            public int Type;
            public MOUSEKEYBDHARDWAREINPUT MouseKeyboardHardwareInput;
        }
    }
}
