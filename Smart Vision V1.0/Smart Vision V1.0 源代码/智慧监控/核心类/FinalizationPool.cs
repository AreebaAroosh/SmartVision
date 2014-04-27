using System;
using System.Collections;
using System.Threading
;
namespace IPCamera
{
	/// <summary>
	/// FinalizationPool
	/// </summary>
	public class FinalizationPool : CollectionBase
	{
		private Thread	thread;
		private ManualResetEvent stopEvent = null;

		// �չ��캯��
		public FinalizationPool()
		{
		}

		// ��ʼ�߳�
		public void Start()
		{
			// �����¼�
            stopEvent = new ManualResetEvent(false);    //���Ϊ true���򽫳�ʼ״̬����Ϊ��ֹ�����Ϊ false���򽫳�ʼ״̬����Ϊ����ֹ��
				
			// �����Ϳ�ʼ���߳�
			thread = new Thread(new ThreadStart(WorkerThread));
			thread.Start();
		}

		// ֹͣ�߳�
		public void Stop()
		{
			if (thread != null)
			{
				// ����ֹͣ�ź�
				stopEvent.Set();      //���¼�״̬����Ϊ��ֹ״̬
				// �ȴ��߳���ֹ
				thread.Join();

				thread = null;

				// �ͷ��¼�
				stopEvent.Close();
				stopEvent = null;
			}
		}

		// �߳̽����
		private void WorkerThread()
		{
			while (!stopEvent.WaitOne(0, true))
			{
				Monitor.Enter(this);

                int n = InnerList.Count;

				// ��ѯÿһ������ͷ
				for (int i = 0; i < n; i++)
				{
					Camera camera = (Camera) InnerList[i];

					if (!camera.Running)
					{
						camera.CloseVideoSource();
						InnerList.RemoveAt(i);
						i--;
						n--;
					}
				}
				Monitor.Exit(this);

                //�ȴ�
				Thread.Sleep(300);
			}

            //�ر�����ͷ
			foreach (Camera camera in InnerList)
			{
				camera.Stop();
			}
		}

		// ����������ͷ
		public void Add(Camera camera)
		{
			Monitor.Enter(this);
			InnerList.Add(camera);
			Monitor.Exit(this);
		}

		// �Ƴ�����ͷ
		public void Remove(Camera camera)
		{
			Monitor.Enter(this);

			int n = InnerList.Count;
			for (int i = 0; i < n; i++)
			{
				if (InnerList[i] == camera)
				{
					if (camera.Running)
						camera.Stop();
					camera.CloseVideoSource();
					InnerList.RemoveAt(i);
					break;
				}
			}

			Monitor.Exit(this);
		}
	}
}
