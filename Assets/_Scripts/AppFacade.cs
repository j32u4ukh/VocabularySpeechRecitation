using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vts.mvc
{
    /// <summary>
    /// PureMVC ���򥻨ϥΪ��I�s�y�{�޲z
    /// 1. �w�q�ۤv�� Facade ���A�~�� Facade ���A���ѳo��������ҼҦ��ݩ� Instance
    /// 2. �ϥΦۤv�� Facade ������ SendNotification ��k�ǰe�q���A��o�Ӥ�k�i��ʸˡC
    /// 3. �@�w�s�b�@�ӵ��U�q�����禡�A�_�h�ۤv�w�q���q���L�k����C�b�ۤv�w�q�� Facade �禡�����g InitializeController ��k�A�b�o�Ӥ�k���I�s RegisterCommand �禡���U�q���C
    /// 4. �w�q��~���U�q���ɪ�^�� Command ���C�۩w�q�� Command ���~�Ӧ� SimpleCommand ���Ϊ� MacroCommand ���]����{�F ICommand �����^�C
    ///    SimpleCommand �������g Execute ��k�A��e Command �ݭn���檺�޿�{���X�N�w�q�b�o�Ӥ�k���F
    ///    MacroCommand �������g InitializeMacroCommand ��k�A�������@�� IList<Func<ICommand>> ���O�� subCommands �ܼơA
    ///    MacroCommand �i�H�����h�� SimpleCommand �Ϊ� MacroCommand�A���x�s�b subCommands �ܼƤ��A���� Execute ��k�w�g�w�q�n�F���έ��g�A
    ///    Execute �禡�|�̦������������Ҧ� SimpleCommand �M MacroCommand�A�b InitializeMacroCommand ��k���q�L AddSubCommand ��k�N Command �[�J subCommands �ܼƧY�i�C
    /// </summary>
    public class AppFacade : PureMVC.Patterns.Facade.Facade
    {
        private static string KEY = "MainFacade";

        public AppFacade() : base(key: KEY)
        {

        }

        // ���ݨ즳���g�k�O��^ IFacade�A�����D�������u���I�H
        public static AppFacade getInstance()
        {
            return GetInstance(KEY, key => new AppFacade()) as AppFacade;
        }

        /// <summary>
        /// ��l��controller�������e
        /// </summary>
        protected override void InitializeController()
        {
            //�i�H�O�d�A��������l�Ʈ�new�F�@��controller
            base.InitializeController();

            // �R�O�M�q��ô�����޿�
            // ���U�q���A������e�U�A�b�禡����^�@�өR�O�A
            registerCommand(ENotification.Init, () =>
            {
                return new InitCommand();
            });
        }

        /// <summary>
        /// �ҰʩR�O���禡�A��L�禡�I�s�o�Ө禡�ҰʩR�O
        /// </summary>
        public void init()
        {
            sendNotification(ENotification.Init);
        }

        #region Model
        /// <summary>
        /// Register an <c>IProxy</c> with the <c>Model</c> by name.
        /// </summary>
        /// <param name="proxy">the <c>IProxy</c> instance to be registered with the <c>Model</c>.</param>
        public void registerProxy(PureMVC.Interfaces.IProxy proxy)
        {
            RegisterProxy(proxy);
        } 
        #endregion

        #region View
        /// <summary>
        /// Register a <c>IMediator</c> with the <c>View</c>.
        /// </summary>
        /// <param name="mediator">a reference to the <c>IMediator</c></param>
        public void registerMediator(PureMVC.Interfaces.IMediator mediator)
        {
            RegisterMediator(mediator);
        } 
        #endregion

        #region Controller
        /// <summary>
        /// Register an <c>ICommand</c> with the <c>Controller</c> by Notification name.
        /// </summary>
        /// <param name="notificationName">the name of the <c>INotification</c> to associate the <c>ICommand</c> with</param>
        /// <param name="commandFunc">a reference to the Class of the <c>ICommand</c></param>
        public void registerCommand(ENotification notification, Func<PureMVC.Interfaces.ICommand> commandFunc)
        {
            RegisterCommand(notification.ToString(), commandFunc);
        }
        #endregion

        #region Notification
        public void sendNotification(ENotification notification, object body = null, string type = null)
        {
            SendNotification(notification.ToString(), body, type);
        } 
        #endregion
    }

}