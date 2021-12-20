using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Animations;
using Microsoft.Maui.Controls;
using System;

namespace Syncfusion.Maui.Core.Internals
{
    /// <summary>
    /// 
    /// </summary>
    public enum CoreSchedulerType
    { 
        /// <summary>
        /// 
        /// </summary>
        Frame,

        /// <summary>
        /// 
        /// </summary>
        Main,

        /// <summary>
        /// 
        /// </summary>
        Composite
    }

    /// <summary>
    /// 
    /// </summary>
    public abstract class CoreScheduler
    {
        private Action? callback;

        /// <summary>
        /// 
        /// </summary>
        public bool IsScheduled { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static CoreScheduler CreateScheduler(CoreSchedulerType type = CoreSchedulerType.Frame)
        {
            switch(type)
            {
                case CoreSchedulerType.Frame:
                    return new CoreFrameScheduler();
                case CoreSchedulerType.Main:
                    return new CoreMainScheduler();
                default:
                    return new CoreCompositeScheduler();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public bool ScheduleCallback(Action action)
        {
            if (action == null)
                throw new ArgumentNullException("action");

            if (!IsScheduled)
            {
                IsScheduled = true;
                callback = action;
                return OnSchedule(InvokeCallback);
            }

            return false;
        }

        private void InvokeCallback()
        {
            if (callback != null)
            {
                callback();
                callback = null;
                IsScheduled = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        protected abstract bool OnSchedule(Action action);
    }

    /// <summary>
    /// 
    /// </summary>
    public class CoreFrameScheduler : CoreScheduler
    {
        private Action? callback;
        private readonly Microsoft.Maui.Animations.Animation animation;
        IAnimationManager? animationManager;

        /// <summary>
        /// 
        /// </summary>
        public CoreFrameScheduler()
        {
            animation = new Microsoft.Maui.Animations.Animation(OnFrameStart, start:0.001f, duration:0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        protected override bool OnSchedule(Action action)
        {
            if (Application.Current != null)
            {
                var handler = Application.Current.Handler;
                if (handler != null && handler.MauiContext != null)
                    animationManager = handler.MauiContext.Services.GetRequiredService<IAnimationManager>();

                if (animationManager != null)
                {
                    callback = action;
                    animation.Reset();
                    animation.Commit(animationManager);
                    return true;
                }
            }
            return false;
        }

        private void OnFrameStart(double value)
        {
            if (callback != null)
            {
                callback();
                callback = null;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class CoreMainScheduler : CoreScheduler
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        protected override bool OnSchedule(Action action)
        {
            Device.BeginInvokeOnMainThread(action);
            return true;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    internal class CoreCompositeScheduler : CoreScheduler
    {
        CoreFrameScheduler frameScheduler = new CoreFrameScheduler();
        CoreMainScheduler mainScheduler = new CoreMainScheduler();
        Action? callback;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        protected override bool OnSchedule(Action action)
        {
            callback = action;
            frameScheduler.ScheduleCallback(InvokeCallback);
            mainScheduler.ScheduleCallback(InvokeCallback);
            return true;
        }

        private void InvokeCallback()
        {
            if (IsScheduled)
            {
                if (callback != null)
                    callback();
            }
        }
    }
}
