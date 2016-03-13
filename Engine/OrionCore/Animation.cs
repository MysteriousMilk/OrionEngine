using Microsoft.Xna.Framework;
using Orion.Core.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Core
{
    public class Animation
    {
        private double _elapsed;
        private bool _doneAnimating;
        private AnimationState _state = AnimationState.Inanimate;

        public string Ref { get; set; }
        public double TimePerFrame { get; set; }
        public int StartFrame { get; set; }
        public int EndFrame { get; set; }
        public bool Loop { get; set; }
        public int CurrentFrame { get; set; }

        public AnimationState State
        {
            get { return _state; }
            set
            {
                if (value != _state)
                {
                    _state = value;

                    if (this.AnimationStateChanged != null)
                        AnimationStateChanged(this, new AnimationStateEventArgs(value));
                }
            }
        }

        public event AnimationStateEventHandler AnimationStateChanged;

        public Animation(string refName, int start, int end, double timePerFrame, bool loop)
        {
            this.Ref = refName;
            this.StartFrame = start;
            this.EndFrame = end;
            this.TimePerFrame = timePerFrame;
            this.Loop = loop;

            Reset();
        }

        public void Reset()
        {
            this._elapsed = 0.0f;
            this.CurrentFrame = StartFrame;
            this._doneAnimating = true;
        }

        public void Play()
        {
            this._elapsed = 0.0f;
            this.CurrentFrame = StartFrame;
            this._doneAnimating = false;
        }

        public void Update(GameTime gameTime)
        {
            // make sure we are still animating
            if (!_doneAnimating)
            {
                this.State = AnimationState.Animate;

                // get the elapsed time since the last update and add it to elapsed
                _elapsed += gameTime.ElapsedGameTime.TotalMilliseconds;

                // when the correct amount of time has past,
                // move to the next frame and reset the elapsed time
                if (_elapsed > TimePerFrame)
                {
                    _elapsed = 0.0f;

                    this.CurrentFrame++;
                    if (this.CurrentFrame > EndFrame)
                    {
                        if (this.Loop)
                            this.CurrentFrame = StartFrame;
                        else
                        {
                            this.CurrentFrame = EndFrame;
                            _doneAnimating = true;
                        }
                    }
                }
            }
            else
            {
                this.State = AnimationState.Inanimate;
            }
        }

        public Animation Clone()
        {
            return this.MemberwiseClone() as Animation;
        }
    }
}
