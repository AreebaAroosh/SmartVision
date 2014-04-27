using System;

namespace IPCamera
{
	/// <summary>
	/// IWizardPage - ��ҳ��ӿ�
	/// </summary>
	public interface IWizardPage
	{
        //״̬�ı��¼�
		event EventHandler StateChanged;

		//��Ϣ�����¼�
		//event EventHandler Reset;

        //ҳ������
		string PageName { get; }

		//��������
		string PageDescription { get; }

		// ��ɣ���ҳ��������ɣ����Խ�����һҳ
		bool Completed { get; }

        // ����ҳ��
		void Display();
        
        // Ӧ������
		bool Apply();
	}
}
