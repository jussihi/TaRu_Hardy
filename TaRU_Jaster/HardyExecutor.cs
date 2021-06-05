using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaRU_Jaster
{
    public class HardyExecutor
    {
        public enum OneShotCommand
        {
            Up,
            Down,
            SetSensitivity,
            SetHitsToFall,
            SetLightsOn,
            SetLightsOff,
            SetMotionOn,
            SetMotionOff,
            SetProgramHitsToFall,
            SetProgramTimeUp,
            SetProgramTimeDown,
            StartProgram,
            EndProgram,
            Reset
        }

        public struct TargetStats
        {
            public bool enabled;
            public int sensitivity;
            public int hitsToFall;
            public bool lightOn;
            public bool motionOn;
            public bool up;
            public float battery;
            public DateTime lastUpdate;
        }


        // Initialized by main form, reference given to this class
        private COMHandler _COMHandler;

        public HardyExecutor(COMHandler w_COMHandler)
        {
            _COMHandler = w_COMHandler;
        }

        public async Task CommandAllJastersReset()
        {
            Global.g_form1.log_msg("RESET all Jasters ...");

            byte[] jastersResetBuffer = { 0x91 };
            await _COMHandler.SendSerial(jastersResetBuffer);
        }

        public async Task CommandAllJastersUp()
        {
            Global.g_form1.log_msg("UP all Jasters ...");

            byte[] jastersUpBuffer = { 0x80 };
            await _COMHandler.SendSerial(jastersUpBuffer);
        }

        public async Task CommandAllJastersDown()
        {
            Global.g_form1.log_msg("DOWN all Jasters ...");

            byte[] jastersDownBuffer = { 0x81 };
            await _COMHandler.SendSerial(jastersDownBuffer);
        }

        private static byte[] AddTargetNumberToAddress(int w_target, byte[] w_addressBytes)
        {
            // place the dummy to right position to create the address
            w_addressBytes[(w_target - 1) / 7] |= (byte)(0b_0000_0001 << ((w_target - 1) % 7));

            // return address array
            return w_addressBytes;
        }

        public async Task<bool> OneShotTargetsSimpleExecute(List<int> w_targets, OneShotCommand w_command, byte w_param = 0xFF)
        {
            // Initialize the needed byte buffers
            byte[] command = null;
            byte[] address = new byte[5];
            byte[] nullBytes = null;

            // set alltargets
            bool allTargets = (w_targets.Count == 30 || w_targets.Count == 0);

            // Set target array (unset if needed in command switch!)
            foreach (int targetNo in w_targets)
            {
                address = AddTargetNumberToAddress(targetNo, address);
            }

            // Set up the command-specific commands
            switch (w_command)
            {
                case OneShotCommand.Up:
                    if (allTargets)
                    {
                        await CommandAllJastersUp();
                        return true;
                    }
                    command = new byte[] { 0x82 };
                    nullBytes = new byte[8];
                    break;
                case OneShotCommand.Down:
                    if (allTargets)
                    {
                        await CommandAllJastersDown();
                        return true;
                    }
                    command = new byte[] { 0x83 };
                    nullBytes = new byte[8];
                    break;
                case OneShotCommand.SetSensitivity:
                    // Check that the sensitivity is within limits
                    if (w_param > 99)
                    {
                        Global.g_form1.log_msg("Sensitivity value not set or invalid: " + w_param);
                        return false;
                    }
                    if (allTargets)
                    {
                        command = new byte[] { 0x92 };
                        address = null;
                        nullBytes = new byte[] { 0x7F, 0x7F, 0x7F, 0x7F, 0x7F, 0x7F, 0x7F,
                                                 0x7F, 0x7F, 0x7F, 0x7F, 0x7F, 0x7F };
                        break;
                    }
                    command = new byte[] { 0x92 };
                    nullBytes = new byte[8];
                    break;
                case OneShotCommand.SetHitsToFall:
                    // Check that the hitstofall is within limits
                    if (w_param > 99 || w_param == 0)
                    {
                        Global.g_form1.log_msg("Number of hits to fall value not set or invalid: " + w_param);
                        return false;
                    }
                    if (allTargets)
                    {
                        command = new byte[] { 0x8C };
                        address = null;
                        nullBytes = new byte[] { 0x7F, 0x7F, 0x7F, 0x7F, 0x7F, 0x7F, 0x7F,
                                                 0x7F, 0x7F, 0x7F, 0x7F, 0x7F, 0x7F };
                        break;
                    }
                    command = new byte[] { 0x8C };
                    nullBytes = new byte[8];
                    break;
                case OneShotCommand.SetLightsOn:
                    if (allTargets)
                    {
                        command = new byte[] { 0x88 };
                        address = null;
                        break;
                    }
                    command = new byte[] { 0x9C };
                    nullBytes = new byte[8];
                    break;
                case OneShotCommand.SetLightsOff:
                    if (allTargets)
                    {
                        command = new byte[] { 0x89 };
                        address = null;
                        break;
                    }
                    command = new byte[] { 0x9D };
                    nullBytes = new byte[8];
                    break;
                case OneShotCommand.SetMotionOn:
                    if (allTargets)
                    {
                        command = new byte[] { 0x9E };
                        address = null;
                        nullBytes = new byte[] { 0x7F, 0x7F, 0x7F, 0x7F, 0x7F, 0x7F, 0x7F,
                                                 0x7F, 0x7F, 0x7F, 0x7F, 0x7F, 0x7F };
                        break;
                    }
                    command = new byte[] { 0x9E };
                    nullBytes = new byte[8];
                    break;
                case OneShotCommand.SetMotionOff:
                    if (allTargets)
                    {
                        command = new byte[] { 0x9F };
                        address = null;
                        nullBytes = new byte[] { 0x7F, 0x7F, 0x7F, 0x7F, 0x7F, 0x7F, 0x7F,
                                                 0x7F, 0x7F, 0x7F, 0x7F, 0x7F, 0x7F };
                        break;
                    }
                    command = new byte[] { 0x9F };
                    nullBytes = new byte[8];
                    break;
                case OneShotCommand.SetProgramHitsToFall:
                    // Check that the hitstofall is within limits
                    if (w_param > 99 || w_param == 0)
                    {
                        Global.g_form1.log_msg("Number of hits to fall value not set or invalid: " + w_param);
                        return false;
                    }
                    if (allTargets)
                    {
                        command = new byte[] { 0x8F };
                        address = new byte[] { 0x7F, 0x7F, 0x7F, 0x7F, 0x03 };
                        nullBytes = new byte[8];
                        break;
                    }
                    command = new byte[] { 0x8F };
                    nullBytes = new byte[8];
                    break;
                case OneShotCommand.SetProgramTimeUp:
                    // Check that the hitstofall is within limits
                    if (w_param > 99)
                    {
                        Global.g_form1.log_msg("Timeup value not set or invalid: " + w_param);
                        return false;
                    }
                    if (allTargets)
                    {
                        command = new byte[] { 0x93 };
                        address = new byte[] { 0x7F, 0x7F, 0x7F, 0x7F, 0x03 };
                        nullBytes = new byte[8];
                        break;
                    }
                    command = new byte[] { 0x93 };
                    nullBytes = new byte[8];
                    break;
                case OneShotCommand.SetProgramTimeDown:
                    // Check that the hitstofall is within limits
                    if (w_param > 99)
                    {
                        Global.g_form1.log_msg("Timedown value not set or invalid: " + w_param);
                        return false;
                    }
                    if (allTargets)
                    {
                        command = new byte[] { 0x94 };
                        address = new byte[] { 0x7F, 0x7F, 0x7F, 0x7F, 0x03 };
                        nullBytes = new byte[8];
                        break;
                    }
                    command = new byte[] { 0x94 };
                    nullBytes = new byte[8];
                    break;
                case OneShotCommand.StartProgram:
                    if (allTargets)
                    {
                        command = new byte[] { 0x95 };
                        address = new byte[] { 0x7F, 0x7F, 0x7F, 0x7F, 0x03 };
                        nullBytes = new byte[8];
                        break;
                    }
                    command = new byte[] { 0x95 };
                    nullBytes = new byte[8];
                    break;
                case OneShotCommand.EndProgram:
                    if (allTargets)
                    {
                        command = new byte[] { 0x8F };
                        address = new byte[] { 0x01, 0x7F, 0x7F, 0x7F, 0x7F, 0x03 };
                        nullBytes = new byte[8];
                        break;
                    }
                    command = new byte[] { 0x96 };
                    nullBytes = new byte[8];
                    break;
                case OneShotCommand.Reset:
                    if (allTargets)
                    {
                        await CommandAllJastersReset();
                        return true;
                    }
                    command = new byte[] { 0x9B };
                    nullBytes = new byte[8];
                    break;
            }

            // compile the final command and send it out
            if (command == null)
            {
                Global.g_form1.log_msg("ERROR: No command given!");
            }

            // append param if it's present
            if (w_param != 0xFF)
            {
                command = command.Concat(new byte[] { w_param }).ToArray();
            }

            // append address if it's present
            if (address != null)
            {
                command = command.Concat(address).ToArray();
            }

            // append null bytes if it's present
            if (nullBytes != null)
            {
                command = command.Concat(nullBytes).ToArray();
            }

            // Finally, execute the command
            await _COMHandler.SendSerial(command);
            return true;
        }

        public async Task<TargetStats?> GetTargetStats(int w_targetNo)
        {
            byte[] command = { 0x90, 0x00 };
            command[1] = (byte)w_targetNo;

            await _COMHandler.SendSerial(command);
            byte[] res = await _COMHandler.ReadSerial(6);
            if (res == null)
                return null;

            // Initialize return struct and return it
            TargetStats ret = new TargetStats();

            // Set states
            bool help;
            help = (res[0] & 0x20) == 0x20 ? (ret.up = true) : (ret.up = false);
            help = (res[0] & 0x08) == 0x08 ? (ret.lightOn = true) : (ret.lightOn = false);

            // Set hits to fall
            ret.hitsToFall = res[1];

            // Bytes 2-3 unknown ???

            // Set battery level
            ret.battery = ((float)((int)res[4]) / 10.0f) + 3.0f;

            // Set sensitivity
            ret.sensitivity = res[5];

            // Set target enabled (since we were able to connect) and latest timestamp
            ret.enabled = true;
            ret.lastUpdate = DateTime.Now;
            return ret;
        }
    }
}
