using System.Collections.Generic;
using Sandbox.ModAPI.Ingame;


namespace IngameScript
{
    partial class Program: IProgram
    {
        private FlyingPlan _plan;
        
        private FlyControler _flyControler;
        
        private IMyShipController _shipController;
        
        private IFlyible _fly;
        public void Init()
        {
            Runtime.UpdateFrequency = UpdateFrequency.Update10;
            
            _shipController = GridTerminalSystem.GetBlockWithName("Remote Control") as IMyShipController;
            _plan = new FlyingPlan();
            
            List<IMyGyro> gyros = new List<IMyGyro>();
            GridTerminalSystem.GetBlocksOfType(gyros);
            _fly = new DirectFlyier(new EchoLogger(Echo), gyros, _shipController);
            
            _flyControler = new FlyControler(_logger, _shipController, _fly);
            
            var blocks = new List<IMyTerminalBlock>();
            GridTerminalSystem.GetBlocks(blocks);
            new AutoTag(Me).SetTag(blocks);
            
            _commandExecutor.Add("AddPoint", args => _plan.Add(new FlyingPoint(GpsParser.Parse(args[0]))));
            _commandExecutor.Add("FinishPlan", () => _flyControler.SetNewPlan(_plan));
            _commandExecutor.Add("StartFlying", () =>
            {
                _flyControler.BeginFlying();
            });
            _commandExecutor.Add("FlyTest", () =>
            {
                _commandExecutor.TryExecute("ClearLogger");
                _commandExecutor.TryExecute("AddPoint", "GPS:AIBaster#3:64085.24:-85776.61:-40876.44:#FF75C9F1:");
                _commandExecutor.TryExecute("FinishPlan");
                _commandExecutor.TryExecute("StartFlying");
            });
        }
        
        public void Execute()
        { 
            _flyControler.Control();
        }
    }
}