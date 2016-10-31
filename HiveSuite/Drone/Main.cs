﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HiveSuite.Core;

namespace HiveSuite.Drone
{
    
   
    /// <summary>
    /// Main loop
    /// </summary>
    public class Drone
    {

        public Network ComObject { get; set; }

        //public Listen ComListener { get; set; }

        public Status CurrentStatus { get; private set; }
        
        public State CurrentState { get; private set; }

        public State DesiredState { get; private set; }

        public Task CurrentTask { get; private set; }

        public DroneSettings Settings { get; set; }

        public void Main()
        {

            CurrentStatus = Status.NotReadyForWork;
            CurrentState = State.StartingUP;
            DesiredState = State.Ready;

            while (CurrentState!=State.ShuttingDown)
            {
                switch (CurrentState)
                {
                    case State.StartingUP:
                        try
                        {
                            // Load The settings 
                            if (LoadSettings() && InitilizeComms())
                            {
                                //If we can communicate we can Talk to the TaskMaster or Queen
                                if (ConnectToTaskMaster())
                                {
                                    //If we can talk to the task master We should be ready to do something
                                    UpdateState(DesiredState);
                                    break;
                                }
                                else
                                {
                                    UpdateState(State.ErrorFault);
                                }
                            }
                            else
                            {
                                UpdateState(State.ShuttingDown);
                            }
                            
                        }
                        catch (Exception e)
                        {

                            UpdateState(State.ShuttingDown);
                            Log("Fail During Startup", e);
                        }

                        break;
                    case State.StartingTask:
                        CurrentStatus = Status.NotReadyForWork;
                        //TODO: start processing the tasks or at least doing the set up
                        break;
                    case State.StoppingTask:
                        //TODO: Start Code to tear down any worker threads or at least acknowledge that the thread is done
                        CurrentStatus = Status.NotReadyForWork;
                        break;
                    case State.CleaningUP:
                        //TODO: clean up any assets left over from the execution
                        CurrentStatus = Status.NotReadyForWork;
                        break;
                    case State.ResettingWorkspace:
                        //TODO: Clean up any working folders 
                        CurrentStatus = Status.NotReadyForWork;
                        break;
                    case State.Restarting:
                        //TODO: code for closing coms and flusing
                        CurrentStatus = Status.NotReadyForWork;
                        break;
                    case State.ShuttingDown:
                        CurrentStatus = Status.NotReadyForWork;
                        break;
                    case State.ErrorFault:
                        //TODO: Need to Log All status and objects before shutting down 
                        CurrentStatus = Status.NotReadyForWork;
                        break;
                    case State.Ready:
                        CurrentStatus = Status.ReadyForWork;
                        break;
                    default:
                        break;
                }


                switch (CurrentStatus)
                {
                    case Status.ReadyForWork:
                        //Connect to TaskMaster and Check for Available Task
                        break;
                    case Status.NotReadyForWork:

                        break;
                    default:
                        break;
                }



            }
            
                       


        }

        private void Log(string v, Exception e)
        {
            throw new NotImplementedException();
        }

        private void UpdateState(State desiredStatus)
        {
            CurrentState = desiredStatus;
        }

        private bool ConnectToTaskMaster()
        {
            ComObject.SendDiscovery();

            if(ComObject.PeerCount < 0)
            {
                return true;
            }

            return false;
        }

        private bool InitilizeComms()
        {
            ComObject = new Network(Settings.Port);

            return ComObject.CommsUp();
        }

        private bool LoadSettings()
        {
            Settings = new DroneSettings();
            Settings.Load(Settings.DefaultPath);

            if(Settings == null)
            {
                return false;
            }

            return true;
        }
        // TODO: Communications - Layer
        // TODO: Talk to Hive - task
        // TODO: Init Binary Cache - task / layer
        // TODO: Execute Task - 
        // TODO: Cleanup - task
        // TODO: Close Comm - task
    }
}
