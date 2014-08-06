using Modbus.Device;

namespace Maxwe11.Modbus.Master
{
    using System;

    /// <summary>
    /// 
    /// </summary>
    public class HoldingRegistersReader
    {
        #region Constants

        /// <summary>
        /// 
        /// </summary>
        public const ushort MaximumRegistersCountPerRequest = 125;

        #endregion

        #region Fields

        private ushort mRegistersCountPerRequest = MaximumRegistersCountPerRequest;

        private readonly IModbusMaster mMaster;

        #endregion

        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        /// <param name="master"></param>
        public HoldingRegistersReader(IModbusMaster master)
        {
            if (master == null)
                throw new ArgumentNullException("master");

            mMaster = master;
            ReadAsLittleEndian = true;
        }

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public IModbusMaster ModbusMaster { get { return mMaster; } }

        /// <summary>
        /// 
        /// </summary>
        public bool ReadAsLittleEndian { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ushort RegistersCountPerRequest
        {
            get { return mRegistersCountPerRequest; }
            set
            {
                if (value <= 0 || value > MaximumRegistersCountPerRequest)
                    throw new ArgumentException("RegistersCountPerRequest should be between 1 and MaximumRegistersCountPerRequest inclusive");

                mRegistersCountPerRequest = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="slaveId"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        public short ReadInt16(byte slaveId, ushort address)
        {
            var register = mMaster.ReadHoldingRegisters(slaveId, address, sizeof(short) / sizeof(ushort))[0];

            if (BitConverter.IsLittleEndian != ReadAsLittleEndian)
                return (short)(((register & 0x00FF) << 8) | ((register & 0xFF00) >> 8));
            
            return (short)register;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="slaveId"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        public int ReadInt32(byte slaveId, ushort address)
        {
            var registers = mMaster.ReadHoldingRegisters(slaveId, address, sizeof(int) / sizeof(ushort));
            var bytes = new byte[sizeof(int)];

            Buffer.BlockCopy(registers, 0, bytes, 0, bytes.Length);
            
            if (BitConverter.IsLittleEndian != ReadAsLittleEndian)
                Array.Reverse(bytes);

            return BitConverter.ToInt32(bytes, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="slaveId"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        public long ReadInt64(byte slaveId, ushort address)
        {
            var registers = mMaster.ReadHoldingRegisters(slaveId, address, sizeof(long) / sizeof(ushort));
            var bytes = new byte[sizeof(long)];

            Buffer.BlockCopy(registers, 0, bytes, 0, bytes.Length);

            if (BitConverter.IsLittleEndian != ReadAsLittleEndian)
                Array.Reverse(bytes);

            return BitConverter.ToInt64(bytes, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="slaveId"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        public ushort ReadUInt16(byte slaveId, ushort address)
        {
            var register = mMaster.ReadHoldingRegisters(slaveId, address, sizeof(short) / sizeof(ushort))[0];

            if (BitConverter.IsLittleEndian != ReadAsLittleEndian)
                return (ushort)(((register & 0x00FF) << 8) | ((register & 0xFF00) >> 8));

            return register;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="slaveId"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        public uint ReadUInt32(byte slaveId, ushort address)
        {
            var registers = mMaster.ReadHoldingRegisters(slaveId, address, sizeof(uint) / sizeof(ushort));
            var bytes = new byte[sizeof(uint)];

            Buffer.BlockCopy(registers, 0, bytes, 0, bytes.Length);

            if (BitConverter.IsLittleEndian != ReadAsLittleEndian)
                Array.Reverse(bytes);

            return BitConverter.ToUInt32(bytes, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="slaveId"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        public ulong ReadUInt64(byte slaveId, ushort address)
        {
            var registers = mMaster.ReadHoldingRegisters(slaveId, address, sizeof(ulong) / sizeof(ushort));
            var bytes = new byte[sizeof(ulong)];

            Buffer.BlockCopy(registers, 0, bytes, 0, bytes.Length);

            if (BitConverter.IsLittleEndian != ReadAsLittleEndian)
                Array.Reverse(bytes);

            return BitConverter.ToUInt64(bytes, 0);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="slaveId"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        public float ReadFloat(byte slaveId, ushort address)
        {
            var registers = mMaster.ReadHoldingRegisters(slaveId, address, sizeof(float) / sizeof(ushort));
            var bytes = new byte[sizeof(float)];

            Buffer.BlockCopy(registers, 0, bytes, 0, bytes.Length);

            if (BitConverter.IsLittleEndian != ReadAsLittleEndian)
                Array.Reverse(bytes);
            
            return BitConverter.ToSingle(bytes, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="slaveId"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        public double ReadDouble(byte slaveId, ushort address)
        {
            var registers = mMaster.ReadHoldingRegisters(slaveId, address, sizeof(double) / sizeof(ushort));
            var bytes = new byte[sizeof(double)];

            Buffer.BlockCopy(registers, 0, bytes, 0, bytes.Length);

            if (BitConverter.IsLittleEndian != ReadAsLittleEndian)
                Array.Reverse(bytes);

            return BitConverter.ToDouble(bytes, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="slaveId"></param>
        /// <param name="address"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public ushort[] ReadRegisters(byte slaveId, ushort address, ushort count)
        {
            var startAddress = address;
            var result = new ushort[count];
            int registersToRead = count;
            ushort[] registers;

            while (registersToRead >= RegistersCountPerRequest)
            {
                registers = mMaster.ReadHoldingRegisters(slaveId, address, RegistersCountPerRequest);
                registers.CopyTo(result, address - startAddress);

                registersToRead -= RegistersCountPerRequest;
                address += RegistersCountPerRequest;
            }

            if (registersToRead > 0)
            {
                registers = mMaster.ReadHoldingRegisters(slaveId, address, (ushort)registersToRead);
                registers.CopyTo(result, address - startAddress);
            }

            return result;
        }

        #endregion
    }
}
