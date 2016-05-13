using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Axiom.Graphics;
using ResourceHandle = System.UInt64;

namespace Axiom.Core
{
    /// <summary>
    ///    Delegate for RenderTarget update events.
    /// </summary>
    public delegate void RenderTargetEventHandler(RenderTargetEventArgs e);

    /// <summary>
    ///    Delegate for Viewport update events.
    /// </summary>
    public delegate void RenderTargetViewportEventHandler(RenderTargetViewportEventArgs e);

    /// <summary>
    ///    Event arguments for render target updates.
    /// </summary>
    public class RenderTargetEventArgs : EventArgs
    {
        internal RenderTexture source;

        public RenderTexture Source { get { return source; } }

        public RenderTargetEventArgs(RenderTexture source)
        {
            this.source = source;
        }
    }

    /// <summary>
    ///    Event arguments for viewport updates while processing a RenderTarget.
    /// </summary>
    public class RenderTargetViewportEventArgs : RenderTargetEventArgs
    {
        internal Rect viewport;

        public Rect Viewport { get { return viewport; } }

        public RenderTargetViewportEventArgs(RenderTexture source, Rect viewport)
            : base(source)
        {
            this.viewport = viewport;
        }
    }


    public class SceneManager
    {
        public Rect CurrentViewport
        {
            get { return new Rect(); }
        }

        public class RenderEventArgs : EventArgs
		{
			public RenderQueueGroupID RenderQueueId;
			public string Invocation;
		}

		public class BeginRenderQueueEventArgs : RenderEventArgs
		{
			public bool SkipInvocation;
		}

		public class EndRenderQueueEventArgs : RenderEventArgs
		{
			public bool RepeatInvocation;
		}


        private readonly ChainedEvent<BeginRenderQueueEventArgs> _queueStartedEvent = new ChainedEvent<BeginRenderQueueEventArgs>();

      
        public event EventHandler<BeginRenderQueueEventArgs> QueueStarted { add { _queueStartedEvent.EventSinks += value; } remove { _queueStartedEvent.EventSinks -= value; } }

        private readonly ChainedEvent<EndRenderQueueEventArgs> _queueEndedEvent = new ChainedEvent<EndRenderQueueEventArgs>();

      
        public event EventHandler<EndRenderQueueEventArgs> QueueEnded { add { _queueEndedEvent.EventSinks += value; } remove { _queueEndedEvent.EventSinks -= value; } }

    }

    /// <summary>
    ///		Delegate for speicfying the method signature for a render queue event.
    /// </summary>
    public delegate bool RenderQueueEvent(RenderQueueGroupID priority);

    /// <summary>
    /// Delegate for FindVisibleObject events
    /// </summary>
    /// <param name="manager"></param>
    /// <param name="stage"></param>
    /// <param name="view"></param>
    public delegate void FindVisibleObjectsEvent(SceneManager manager, IlluminationRenderStage stage, Rect view);

    // ==============

    public interface IConfigurable
    {
    }

    public interface IPlugin
    {
    }

    public interface IManualResourceLoader
    {
    }

    public class LightList : List<Light>
    {
    }

    public class PassList : List<Axiom.Graphics.Pass>
    {
    }
     
    public class TextureUnitStateList : List<Axiom.Graphics.TextureUnitState>
    {
    }

    public class TextureEffectList : List<Axiom.Graphics.TextureEffect>
    {
    }
     
    public class TechniqueList : List<Axiom.Graphics.Technique>
    {
    }
    public class LodValueList : List<float> { }

    //public class ScriptablePropertyAttribute : Attribute
    //{
    //    public ScriptablePropertyAttribute(string name)
    //    {
    //    }
    //}
    //public class ScriptableProperty : Attribute
    //{
    //    public ScriptableProperty(string name)
    //    {
    //    }
    //}
}
