﻿using System;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using TaskQueueDemo.Task;
using TaskQueueDemo.TaskQueue;

namespace TaskQueueDemo
{
    public partial class MainForm : Form
    {
        TaskQueueV2<MessageTask> MessageQueueV2 = new TaskQueueV2<MessageTask>("消息任务队列V2");
        TaskQueue<MessageTask> MessageQueue = new TaskQueue<MessageTask>("消息任务队列");

        public MainForm()
        {
            this.InitializeComponent();

            Console.WriteLine("注册事件...");
            this.FormClosing += (s, v) =>
            {
                this.MessageQueueV2.Stop();
                this.MessageQueueV2.Dispose();
                this.MessageQueue.Stop();
                this.MessageQueue.Dispose();
            };

            #region 任务队列事件

            this.MessageQueue.TaskEnqueued += new EventHandler<MessageTask>((s, v) =>
            {
                this.Invoke(new Action(() => { this.Text = this.MessageQueue.TaskCount.ToString(); }));
                Console.WriteLine($"<{((TaskQueue<MessageTask>)s).Name}> 入队了新任务：[{v.Name}]");
            });

            this.MessageQueue.TaskDequeued += new EventHandler((s, v) =>
            {
                this.Invoke(new Action(() => { this.Text = this.MessageQueue.TaskCount.ToString(); }));
                Console.WriteLine($"<{((TaskQueue<MessageTask>)s).Name}>有任务出队，剩余任务个数：[{((TaskQueue<MessageTask>)s).TaskCount}]");
            });

            this.MessageQueue.Idle += new EventHandler((s, v) =>
            {
                Console.WriteLine($"<{((TaskQueue<MessageTask>)s).Name}> 进入空闲状态 ...");
            });

            this.MessageQueue.QueueStarted += new DoWorkEventHandler((s, v) =>
            {
                Console.WriteLine($"<{((TaskQueue<MessageTask>)s).Name}> 任务开始执行 ...");
            });

            this.MessageQueue.QueueStoped += new RunWorkerCompletedEventHandler((s, v) =>
            {
                Console.WriteLine($"<{((TaskQueue<MessageTask>)s).Name}> 任务停止执行 ...");
            });
            #endregion

            #region 任务队列V2事件
            this.MessageQueue.TaskEnqueued += new EventHandler<MessageTask>((s, v) =>
            {
                this.Invoke(new Action(() => { this.Text = this.MessageQueue.TaskCount.ToString(); }));
                Console.WriteLine($"<{((TaskQueue<MessageTask>)s).Name}> 入队了新任务：[{v.Name}]");
            });

            this.MessageQueue.TaskDequeued += new EventHandler((s, v) =>
            {
                this.Invoke(new Action(() => { this.Text = this.MessageQueue.TaskCount.ToString(); }));
                Console.WriteLine($"<{((TaskQueue<MessageTask>)s).Name}>有任务出队，剩余任务个数：[{((TaskQueue<MessageTask>)s).TaskCount}]");
            });

            this.MessageQueue.Idle += new EventHandler((s, v) =>
            {
                Console.WriteLine($"<{((TaskQueue<MessageTask>)s).Name}> 进入空闲状态 ...");
            });

            this.MessageQueue.QueueStarted += new DoWorkEventHandler((s, v) =>
            {
                Console.WriteLine($"<{((TaskQueue<MessageTask>)s).Name}> 任务开始执行 ...");
            });

            this.MessageQueue.QueueStoped += new RunWorkerCompletedEventHandler((s, v) =>
            {
                Console.WriteLine($"<{((TaskQueue<MessageTask>)s).Name}> 任务停止执行 ...");
            });
            #endregion
        }

        #region 任务队列控制

        private void Enqueuebutton_Click(object sender, EventArgs e)
        {
            //测试并发任务入队
            Console.WriteLine("MessageQueue 并行入队 10 个任务...");
            Parallel.For(0, 10, new Action<int>(index =>
            {
                this.MessageQueue.Enqueue(new MessageTask($"消息任务-{index}"));
            }));
            Console.WriteLine($"MessageQueue 队列内任务数量：{this.MessageQueue.TaskCount}");
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            this.MessageQueue.Start();
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            this.MessageQueue.Stop();
        }
        #endregion

        #region V2任务控制

        private void V2StartButton_Click(object sender, EventArgs e)
        {
            this.MessageQueueV2.Start();
        }

        private void V2StopButton_Click(object sender, EventArgs e)
        {
            this.MessageQueueV2.Stop();
        }

        private void V2Enqueuebutton_Click(object sender, EventArgs e)
        {
            //测试并发任务入队
            Console.WriteLine("MessageQueueV2 并行入队 10 个任务...");
            Parallel.For(0, 1000, new Action<int>(index =>
            {
                this.MessageQueueV2.Enqueue(new MessageTask($"消息任务-{index}"));
            }));
            Console.WriteLine($"MessageQueueV2 队列内任务数量：{this.MessageQueueV2.TaskCount}");
        }
        #endregion
    }
}
