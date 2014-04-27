using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using videosource;
using Tiger.Video.VFW;

namespace IPCamera
{
    /// <summary>
    /// Camera ��Ƶ��
    /// </summary>
    public class Camera
    {
        private int id = 0;         //��ţ����֣����ܣ�������ã���Ƶ�ṩ�̣���Ƶ��Դ�����һ֡
        private string name;
        private string description = "";
        private object configuration = null;
        private VideoProvider provider = null;
        private IVideoSource videoSource = null;
        private bool photoing = false;

        private Bitmap lastFrame = null;
        private Bitmap recordFrame = null;
        private int width = -1, height = -1;      //����

        AVIWriter aviWriter = new AVIWriter();    //avi��Ƶ¼��

        // ��֡�¼�
        public event EventHandler NewFrame;

        // URL����
        public string URL
        {
            get { return videoSource.VideoSource; }
        }

        // �û�������
        public string Login
        {
            get { return videoSource.Login; }
        }

        // ��������
        public string Password
        {
            get { return videoSource.Password; }
        }

        // �������
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        // ��������
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        // ��������
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        // ��������
        public object Configuration
        {
            get { return configuration; }
            set { configuration = value; }
        }

        // ��ƵԴ����
        public VideoProvider Provider
        {
            get { return provider; }
            set { provider = value; }
        }

        // ���һ֡
        public Bitmap LastFrame
        {
            get { return lastFrame; }
        }

        // �������
        public int Width
        {
            get { return width; }
        }

        // �߶�����
        public int Height
        {
            get { return height; }
        }

        // ֡�յ�����
        public int FramesReceived
        {
            get { return (videoSource == null) ? 0 : videoSource.FramesReceived; }
        }

        // �����յ�����
        public int BytesReceived
        {
            get { return (videoSource == null) ? 0 : videoSource.BytesReceived; }
        }

        // �߳�״̬����
        public bool Running
        {
            get { return (videoSource == null) ? false : videoSource.Running; }
        }

        /// <summary>
        /// ��������
        /// </summary>
        // ���캯��
        public Camera(string name)
        {
            this.name = name;
        }

        // ������ƵԴ
        public bool CreateVideoSource()
        {
            if ((provider != null) && ((videoSource = provider.CreateVideoSource(configuration)) != null))
            {
                // ��video_NewFrameע�ᵽNewFrame�¼���
                videoSource.NewFrame += new CameraEventHandler(video_NewFrame);
                return true;
            }
            return false;
        }

        // �ر���ƵԴ
        public void CloseVideoSource()
        {
            if (videoSource != null)
            {
                videoSource = null;
            }
            // �ͷ����һ֡ռ�õ���Դ
            if (lastFrame != null)
            {
                lastFrame.Dispose();
                lastFrame = null;
            }
            width = -1;
            height = -1;
        }

        // ��ʼ��ƵԴ
        public void Start()
        {
            if (videoSource != null)
            {
                videoSource.Start();
            }
        }

        // ֪ͨ��ƵԴֹͣ
        public void SignalToStop()
        {
            if (videoSource != null)
            {
                videoSource.SignalToStop();
            }
        }

        // �ȴ���ƵԴ��ֹ
        public void WaitForStop()
        {
            Monitor.Enter(this);
            if (videoSource != null)
            {
                videoSource.WaitForStop();
            }
            Monitor.Exit(this);
        }

        // ��ֹ��Ƶ
        public void Stop()
        {
            Monitor.Enter(this);
            if (videoSource != null)
            {
                videoSource.Stop();
            }
            Monitor.Exit(this);
        }

        // ����
        public void Lock()
        {
            Monitor.Enter(this);
        }

        // ����
        public void Unlock()
        {
            Monitor.Exit(this);
        }

        // NewFrame�¼������˺���
        private void video_NewFrame(object sender, CameraEventArgs e)
        {
            // �̼߳���
            Monitor.Enter(this);
            // �����֡
            if (lastFrame != null)
            {
                lastFrame.Dispose();
            }
            // ����ͼƬ
            lastFrame = (Bitmap)e.Bitmap.Clone();
            recordFrame = lastFrame;
            width = lastFrame.Width;
            height = lastFrame.Height;

            if (�ǻۼ��.detector != null && �ǻۼ��.luzhi1 == false)
            {
                �ǻۼ��.motionLevel = �ǻۼ��.detector.ProcessFrame(lastFrame);
            }

            if (photoing == true)
            {
                photoing = false;
                DateTime date = DateTime.Now;
                String fileName = String.Format("{0}-{1}-{2} {3}-{4}-{5}", date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second);
                recordFrame.Save(String.Format(".\\SmartVision\\ͼƬ\\Camera_Vision_{0}_{1}.jpg", name, fileName), ImageFormat.Jpeg);
            }

            // ����
            Monitor.Exit(this);

            // ֪ͨ�ͻ���  camera�Լ�����֡�¼�
            if (NewFrame != null)
            {
                NewFrame(this, new EventArgs());
            }
        }

        //��Ƶ¼�ƺ���
        public void RecordOpen()
        {
            DateTime date = DateTime.Now;
            String fileName = String.Format("{0}-{1}-{2} {3}-{4}-{5}", date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second);
            aviWriter.Open(String.Format(".\\SmartVision\\��Ƶ\\Camera_Vision_{0}_{1}.avi", name, fileName), width, height);
            aviWriter.FrameRate = 25;
            aviWriter.Quality = 100;
        }

        //����Ƶ�������֡
        public void addFrame()
        {
            Monitor.Enter(this);
            aviWriter.AddFrame(recordFrame);
            Monitor.Exit(this);
        }

        //�ر�¼��
        public void RecordClose()
        {
            Thread.Sleep(500);   // ��ֹ�ڹر�ʱ��aviWriter.AddFrame(lastFrame);����
            aviWriter.Close();
            aviWriter.Dispose();
        }

        //����
        public void TakePhoto()
        {
            photoing = true;
        }

    }
}
