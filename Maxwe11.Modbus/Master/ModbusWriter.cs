using Modbus.Device;

namespace Maxwe11.Modbus.Master
{
    using System;
    using System.Linq;

    /// <summary>
    /// 
    /// </summary>
    public class ModbusWriter
    {
        #region Constants

        /// <summary>
        /// 
        /// </summary>
        public const ushort MaximumRegistersCountPerRequest = 123;

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
        public ModbusWriter(IModbusMaster master)
        {
            if (master == null)
                throw new ArgumentNullException("master");

            mMaster = master;
            WriteAsLittleEndian = true;
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
        public bool WriteAsLittleEndian { get; set; }

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
        /// <param name="value"></param>
        public void Write(byte slaveId, ushort address, short value)
        {
            mMaster.WriteSingleRegister(slaveId, address, (ushort)value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="slaveId"></param>
        /// <param name="address"></param>
        /// <param name="value"></param>
        public void Write(byte slaveId, ushort address, int value)
        {
            var registers = new ushort[sizeof(int) / sizeof(ushort)];
            var bytes = BitConverter.GetBytes(value);

            if (BitConverter.IsLittleEndian != WriteAsLittleEndian)
                Array.Reverse(bytes);

            Buffer.BlockCopy(bytes, 0, registers, 0, bytes.Length);
            mMaster.WriteMultipleRegisters(slaveId, address, registers);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="slaveId"></param>
        /// <param name="address"></param>
        /// <param name="value"></param>
        public void Write(byte slaveId, ushort address, long value)
        {
            var registers = new ushort[sizeof(long) / sizeof(ushort)];
            var bytes = BitConverter.GetBytes(value);

            if (BitConverter.IsLittleEndian != WriteAsLittleEndian)
                Array.Reverse(bytes);

            Buffer.BlockCopy(bytes, 0, registers, 0, bytes.Length);
            mMaster.WriteMultipleRegisters(slaveId, address, registers);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="slaveId"></param>
        /// <param name="address"></param>
        /// <param name="value"></param>
        public void Write(byte slaveId, ushort address, ushort value)
        {
            mMaster.WriteSingleRegister(slaveId, address, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="slaveId"></param>
        /// <param name="address"></param>
        /// <param name="value"></param>
        public void Write(byte slaveId, ushort address, uint value)
        {
            var registers = new ushort[sizeof(uint) / sizeof(ushort)];
            var bytes = BitConverter.GetBytes(value);

            if (BitConverter.IsLittleEndian != WriteAsLittleEndian)
                Array.Reverse(bytes);

            Buffer.BlockCopy(bytes, 0, registers, 0, bytes.Length);
            mMaster.WriteMultipleRegisters(slaveId, address, registers);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="slaveId"></param>
        /// <param name="address"></param>
        /// <param name="value"></param>
        public void Write(byte slaveId, ushort address, ulong value)
        {
            var registers = new ushort[sizeof(ulong) / sizeof(ushort)];
            var bytes = BitConverter.GetBytes(value);

            if (BitConverter.IsLittleEndian != WriteAsLittleEndian)
                Array.Reverse(bytes);

            Buffer.BlockCopy(bytes, 0, registers, 0, bytes.Length);
            mMaster.WriteMultipleRegisters(slaveId, address, registers);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="slaveId"></param>
        /// <param name="address"></param>
        /// <param name="value"></param>
        public void Write(byte slaveId, ushort address, float value)
        {
            var registers = new ushort[sizeof(float) / sizeof(ushort)];
            var bytes = BitConverter.GetBytes(value);

            if (BitConverter.IsLittleEndian != WriteAsLittleEndian)
                Array.Reverse(bytes);

            Buffer.BlockCopy(bytes, 0, registers, 0, bytes.Length);
            mMaster.WriteMultipleRegisters(slaveId, address, registers);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="slaveId"></param>
        /// <param name="address"></param>
        /// <param name="value"></param>
        public void Write(byte slaveId, ushort address, double value)
        {
            var registers = new ushort[sizeof(double) / sizeof(ushort)];
            var bytes = BitConverter.GetBytes(value);

            if (BitConverter.IsLittleEndian != WriteAsLittleEndian)
                Array.Reverse(bytes);

            Buffer.BlockCopy(bytes, 0, registers, 0, bytes.Length);
            mMaster.WriteMultipleRegisters(slaveId, address, registers);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="slaveId"></param>
        /// <param name="address"></param>
        /// <param name="registers"></param>
        public void Write(byte slaveId, ushort address, ushort[] registers)
        {
            if (registers == null)
                throw new ArgumentNullException("registers");

            int offSet = 0;

            while ((registers.Length - offSet) >= RegistersCountPerRequest)
            {
                var packet = registers.Skip(offSet).Take(RegistersCountPerRequest).ToArray();

                mMaster.WriteMultipleRegisters(slaveId, address, packet);
                
                offSet += RegistersCountPerRequest;
                address += RegistersCountPerRequest;
            }

            if (registers.Length > offSet)
            {
                var packet = registers.Skip(offSet).ToArray();
                mMaster.WriteMultipleRegisters(slaveId, address, packet);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="slaveId"></param>
        /// <param name="address"></param>
        /// <param name="coil"></param>
        public void Write(byte slaveId, ushort address, bool coil)
        {
            mMaster.WriteSingleCoil(slaveId, address, coil);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="slaveId"></param>
        /// <param name="address"></param>
        /// <param name="coils"></param>
        public void Write(byte slaveId, ushort address, bool[] coils)
        {
            mMaster.WriteMultipleCoils(slaveId, address, coils);
        }

        #endregion
    }
}
