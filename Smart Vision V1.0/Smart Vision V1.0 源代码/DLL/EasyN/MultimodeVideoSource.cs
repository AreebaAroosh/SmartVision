using System;
using System.Collections;
using videosource;
using jpeg;
using mjpeg;

namespace EasyN
{
	/// <summary>
	/// MultimodeVideoSource�� - ֧�ֶ���ģʽ����ƵԴ
	/// </summary>
	public abstract class MultimodeVideoSource : IVideoSource
	{
		protected IVideoSource	videoSource;
		protected StreamType	streamType;
		private ArrayList		delegates = new ArrayList();

		// ��֡�¼�
		public event CameraEventHandler NewFrame
		{
			add
			{
				videoSource.NewFrame += value;
				delegates.Add((object) value);
			}
			remove
			{
				videoSource.NewFrame -= value;
				delegates.Remove((object) value);
			}
		}

		// ���캯��
		public MultimodeVideoSource()
		{
		}

		// ��Ƶ��������
		public virtual StreamType StreamType
		{
			get { return streamType; }
			set
			{
				// �����ƵԴ�������ı�������
				if ((streamType != value) && (!videoSource.Running))
				{
					streamType = value;
					
					// ��������
					object	userData = videoSource.UserData;
					string	login = videoSource.Login;
					string	password = videoSource.Password;

					// ��������ƵԴ
					switch (streamType)
					{
						case StreamType.Jpeg:
							videoSource = new JPEGSource();
							break;
						case StreamType.MJpeg:
							videoSource = new MJPEGSource();
							break;
					}

					// ��������
					videoSource.Login		= login;
					videoSource.Password	= password;
					videoSource.UserData	= userData;

					// ����֡�¼���ί��
					foreach (object handler in delegates)
						videoSource.NewFrame += (CameraEventHandler) handler;

					UpdateVideoSource();
				}
			}
		}

        // ��ƵԴ����
		public abstract string VideoSource
		{
			get;
			set;
		}

        // �û�������
		public string Login
		{
			get { return videoSource.Login; }
			set { videoSource.Login = value; }
		}

        // ��������
		public string Password
		{
			get { return videoSource.Password; }
			set { videoSource.Password = value; }
		}

        // �յ�֡����
		public int FramesReceived
		{
			get { return videoSource.FramesReceived; }
		}

        // �յ���������
		public int BytesReceived
		{
			get { return videoSource.BytesReceived; }
		}

        // �û���������
		public object UserData
		{
			get { return videoSource.UserData; }
			set { videoSource.UserData = value; }
		}

        // ��ȡ��ƵԴ�̵߳�״̬
		public bool Running
		{
			get { return videoSource.Running; }
		}

		// ��ʼ������Ƶ֡
		public void Start()
		{
			videoSource.Start();
		}

		// ֹͣ������Ƶ֡
		public void SignalToStop()
		{
			videoSource.SignalToStop();
		}

		// �ȴ�ֹͣ
		public void WaitForStop()
		{
			videoSource.WaitForStop();
		}

		// ֹͣ����
		public void Stop()
		{
			videoSource.Stop();
		}

		// ������ƵԴ
		protected abstract void UpdateVideoSource();
	}
}
