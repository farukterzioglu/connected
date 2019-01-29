using System;
using System.Collections.Generic;
using System.Linq;
using Connected.DAL;
using Connected.DAL.Repositories.Interfaces;


using Ninject;

namespace ReceiveAdapterBussinessLayer
{
    public class ReceiveAdapterBL
    {
        private readonly IReceiveAdapterRepository _receiveAdapterRepository;
        private readonly string _registrationKey;
        [Inject] //Dependency injection of DAL will be handle by Ninject
        public ReceiveAdapterBL(IReceiveAdapterRepository receiveAdapterDAL, string registrationKey)
        {
            _registrationKey = registrationKey;

            //Set Data Access Layer
            _receiveAdapterRepository = receiveAdapterDAL;
        }

        public void ReceiveActivityList(List<Activity> activityList)
        {
            foreach (var activity in activityList)
            {
                //Check for registration key
                if (activity.Item == null)
                {
                    continue;
                    //TODO : Log unsuccesfull reading
                }

                if (activity.ActivityType == null)
                {
                    continue;
                    //TODO : Log unsuccesfull reading
                }

                //Item registration
                if (activity.ActivityType.TypeName == "RegisterItem" ||
                    activity.ActivityType.TypeName == "RegisterReadItem" )
                {
                    var registrationInfo = _receiveAdapterRepository.GetItemRegistration(activity.Item.OriginalId);

                    //Item alreday registered, continue
                    if (registrationInfo != null) continue;

                    //Check for registration key
                    if (activity.Item.ItemRegistration == null)
                    {
                        continue;
                        //TODO : Log unsuccesfull registration
                    }
                    //TODO : move registrationKey to Activity from Activity.Item.ItemRegistration
                    else if (activity.Item.ItemRegistration.RegistrationKey != _registrationKey)
                    {
                        continue;
                        //TODO : Log unregistrated activity
                    }

                    int regId;
                    switch (activity.ActivityType.TypeName)
                    {
                        case "RegisterItem":
                            regId = _receiveAdapterRepository.RegisterItem(activity);
                            break;
                        case "RegisterReadItem":
                            regId = _receiveAdapterRepository.RegisterReadItem(activity);
                            break;
                    }

                    //TODO : Log registration process
                }

                //Item Reading 
                else if (activity.ActivityType.TypeName == "ReadItem")
                {
                    //TODO : check registration
                    var registrationInfo = _receiveAdapterRepository.GetItemRegistration(activity.Item.OriginalId);

                    if (registrationInfo != null 
                        && activity.Item != null 
                        && activity.Item.Adjustable != null)
                    {
                        _receiveAdapterRepository.ReadItem(activity);
                    }
                    else
                    {
                        //TODO : Log unregistrated item reading 
                    }
                }
            }
        }
    }
}
