using AudioSwitcher.Util;
using Microsoft.Win32;
using System;
using System.ServiceProcess;
using System.Threading;

namespace AudioSwitcher.Services
{
    public class AudioService
    {
        public StateType GetCurrentState()
        {
            try
            {
                using (var registry = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Multimedia\Audio"))
                {
                    var value = (int)registry.GetValue("AccessibilityMonoMixState");

                    switch (value)
                    {
                        case 0:
                            return StateType.Stereo;
                        case 1:
                            return StateType.Mono;
                        default:
                            return StateType.Unknown;
                    }
                }
            }
            catch
            {
                return StateType.Unknown;
            }
        }

        public void SetCurrentState(StateType currentState, StateType newState)
        {
            var currentValue = currentState.ToRegistryValue() ?? throw new ArgumentException($"Modo de áudio do windows é inválido: '{currentState}'.", nameof(currentState));
            var newValue = newState.ToRegistryValue() ?? throw new ArgumentException($"Não é possível definir o modo de áudio do windows para '{newState}'.", nameof(newState));

            if (!this.SetAudioRegistry(newValue))
                throw new Exception("Não foi possível definir o modo de aúdio do windows.");

            if (!this.RestartAudioService())
            {
                if (this.SetAudioRegistry(currentValue))
                    throw new Exception("Não foi possível definir o modo de aúdio do windows.");
                else
                    throw new Exception("Não foi possível reiniciar o serviço de audio do windows. Reinicie manualmente para aplicar as mudanças.");
            }
        }

        private bool SetAudioRegistry(int state)
        {
            try
            {
                using (var registry = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Multimedia\Audio", true))
                    registry.SetValue("AccessibilityMonoMixState", state);

                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool RestartAudioService()
        {
            try
            {
                var continarExecutando = true;
                var tentativas = 0;

                executar:

                if (tentativas > 5)
                    return false;

                using (var controller = new ServiceController { ServiceName = "audiosrv" })
                {
                    tentativas++;
                    switch (controller.Status)
                    {
                        case ServiceControllerStatus.Running:
                            controller.Stop();
                            break;
                        case ServiceControllerStatus.Stopped:
                            controller.Start();
                            continarExecutando = false;
                            break;
                        case ServiceControllerStatus.ContinuePending:
                        case ServiceControllerStatus.Paused:
                        case ServiceControllerStatus.PausePending:
                        case ServiceControllerStatus.StartPending:
                        case ServiceControllerStatus.StopPending:
                        default:
                            Thread.Sleep(500);
                            break;
                    }

                    if (continarExecutando)
                        goto executar;

                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}