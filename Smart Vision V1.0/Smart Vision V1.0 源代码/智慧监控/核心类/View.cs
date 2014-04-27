using System;


namespace IPCamera
{
	/// <summary>
	/// View ��ͼ��
	/// </summary>
	public class View
	{
		private int		id = 0;
		private string	name;
		private string	description = "";

		private short	cols = 2;
		private short	rows = 2;
		private short	cellWidth = 320;
		private short	cellHeight = 240;

		private int[,]	cameraIDs = new int[3, 3];

        public const int MaxRows = 3;
        public const int MaxCols = 3;

		// ID �������
		public int ID
		{
			get { return id; }
			set { id = value; }
		}	

		// Name ��������
		public string Name
		{
			get { return name; }
			set { name = value; }
		}

		// ID ��������
		public string Description
		{
			get { return description; }
			set { description = value; }
		}

		// Cols ������
		public short Cols
		{
			get { return cols; }
            set { cols = Math.Max((short)1, Math.Min((short)MaxCols, value)); }
		}

		// Rows ������
		public short Rows
		{
			get { return rows; }
			set { rows = Math.Max((short) 1, Math.Min((short)MaxRows, value)); }
		}

        // CellWidth ��ҳ��� 50~800
		public short CellWidth 
		{
			get { return cellWidth; }
			set { cellWidth = Math.Max((short) 50, Math.Min((short) 800, value)); }
		}

        // CellHeight ��ҳ��� 50~800
		public short CellHeight
		{
			get { return cellHeight; }
			set { cellHeight = Math.Max((short) 50, Math.Min((short) 800, value)); }
		}
       
        /// <summary>
        /// ��������
        /// </summary>
		// ���캯��
		public View(string name)
		{
			this.name = name;
		}

		// ��������ͷ����ҳ���Ӧ
		public void SetCamera(int row, int col, int cameraID)
		{
			cameraIDs[row, col] = cameraID;
		}

		// �������ж�λ����ͷ
		public int GetCamera(int row, int col)
		{
			if ((row >= 0) && (col >= 0) && (row < MaxRows) && (col < MaxCols))
			{
				return cameraIDs[row, col];
			}
			return -1;
		}
	}
}
