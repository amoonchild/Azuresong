//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2023/10/07 15:29:59
//------------------------------------------------------------
using GameFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityGameFramework.Runtime;
using Azuresong.Runtime;


namespace SarsGame.Runtime
{
    /// <summary>
    /// 场景内交互npc表.
    /// </summary>
    public class DRInteractiveNpc : DataRowBase
    {
        private int m_Id = 0;

        /// <summary>
        /// 获取实体编号.
        /// </summary>
        public override int Id
        {
            get
            {
                return m_Id;
            }
        }

            /// <summary>
        /// 获取资源名称.
        /// </summary>
        public string AssetName
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取可交互.
        /// </summary>
        public bool Interactive
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取交互范围.
        /// </summary>
        public float Range
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取动作.
        /// </summary>
        public string AnimName
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取楼层.
        /// </summary>
        public string Floor
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取坐标.
        /// </summary>
        public Vector3 Position
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取旋转.
        /// </summary>
        public Vector3 Rotation
        {
            get;
            private set;
        }

            public override bool ParseDataRow(string dataRowString, object userData)
        {
            string[] columnStrings = dataRowString.Split(ASDataTableExtension.DataSplitSeparators);
            for (int i = 0; i < columnStrings.Length; i++)
            {
                columnStrings[i] = columnStrings[i].Trim(ASDataTableExtension.DataTrimSeparators);
            }

            int index = 0;
            index++;
            m_Id = int.Parse(columnStrings[index++]);
            index++;
            AssetName = columnStrings[index++];
            Interactive = bool.Parse(columnStrings[index++]);
            Range = float.Parse(columnStrings[index++]);
            AnimName = columnStrings[index++];
            Floor = columnStrings[index++];
            Position = ASDataTableExtension.ParseVector3(columnStrings[index++]);
            Rotation = ASDataTableExtension.ParseVector3(columnStrings[index++]);

            GeneratePropertyArray();
            return true;
        }

        public override bool ParseDataRow(byte[] dataRowBytes, int startIndex, int length, object userData)
        {
            using (MemoryStream memoryStream = new MemoryStream(dataRowBytes, startIndex, length, false))
            {
                using (BinaryReader binaryReader = new BinaryReader(memoryStream, Encoding.UTF8))
                {
                    m_Id = binaryReader.Read7BitEncodedInt32();
                    AssetName = binaryReader.ReadString();
                    Interactive = binaryReader.ReadBoolean();
                    Range = binaryReader.ReadSingle();
                    AnimName = binaryReader.ReadString();
                    Floor = binaryReader.ReadString();
                    Position = binaryReader.ReadVector3();
                    Rotation = binaryReader.ReadVector3();
                }
            }

            GeneratePropertyArray();
            return true;
        }

            private void GeneratePropertyArray()
        {

        }
    }
}