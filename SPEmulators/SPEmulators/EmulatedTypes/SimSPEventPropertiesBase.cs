namespace SPEmulators.EmulatedTypes
{
    using System;
    using Microsoft.QualityTools.Testing.Fakes.Instances;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Fakes;

    internal class SimSPEventPropertiesBase : Isolator<SPEventPropertiesBase, ShimSPEventPropertiesBase>
    {
        private SPEventReceiverStatus status;
        private SPEventReceiverType eventType;

        public string ErrorMessage
        {
            get;
            set;
        }

        public SPEventReceiverStatus Status
        {
            get
            {
                return this.status;
            }
            set
            {
                if (!Enum.IsDefined(typeof(SPEventReceiverStatus), value))
                {
                    throw new ArgumentException("value");
                }

                this.status = value;
            }
        }

        public string ReceiverData
        {
            get;
            set;
        }

        public SPEventReceiverType EventType
        {
            get
            {
                return this.eventType;
            }
            set
            {
                if (!Enum.IsDefined(typeof(SPEventReceiverType), value))
                {
                    throw new ArgumentException("value");
                }

                this.eventType = value;
            }
        }
        public string RedirectUrl
        {
            get;
            set;
        }

        public Guid SiteId
        {
            get;
            set;
        }

        public bool Cancel
        {
            get
            {
                return (SPEventReceiverStatus)0 != this.Status;
            }
            set
            {
                this.Status = (value ? (SPEventReceiverStatus)2 : (SPEventReceiverStatus)0);
            }
        }

        protected SimSPEventPropertiesBase(SPEventPropertiesBase instance)
            : base(instance)
        {
            this.Status = (SPEventReceiverStatus)0;
            base.Fake.ErrorMessageGet = (() => this.ErrorMessage);
            base.Fake.ErrorMessageSetString = (delegate(string value)
            {
                this.ErrorMessage = value;
            });
            base.Fake.StatusGet = (() => this.Status);
            base.Fake.StatusSetSPEventReceiverStatus = (delegate(SPEventReceiverStatus value)
            {
                this.Status = value;
            });
            base.Fake.CancelGet = (() => this.Cancel);
            base.Fake.CancelSetBoolean = (delegate(bool value)
            {
                this.Cancel = value;
            });
            base.Fake.ReceiverDataGet = (() => this.ReceiverData);
            base.Fake.EventTypeGet = (() => this.EventType);
            base.Fake.EventTypeSetSPEventReceiverType = (delegate(SPEventReceiverType value)
            {
                this.EventType = value;
            });
            base.Fake.RedirectUrlGet = (() => this.RedirectUrl);
            base.Fake.RedirectUrlSetString = (delegate(string value)
            {
                this.RedirectUrl = value;
            });
            base.Fake.SiteIdGet = (() => this.SiteId);
            base.Fake.SiteIdSetGuid = (delegate(Guid value)
            {
                this.SiteId = value;
            });
        }

        public static SimSPEventPropertiesBase FromInstance(SPEventPropertiesBase instance)
        {
            return InstancedPool.CastAsInstanced<SPEventPropertiesBase, SimSPEventPropertiesBase>(instance);
        }

        internal static void Initialize()
        {
            ShimSPEventPropertiesBase.BehaveAsNotImplemented();
        }
    }
}
