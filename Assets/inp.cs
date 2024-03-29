//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/Input/Player.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @inp: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @inp()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Player"",
    ""maps"": [
        {
            ""name"": ""GamePlay"",
            ""id"": ""d13379ec-6370-49f3-a84f-015941f8abd1"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""6b6028fa-9801-4297-b231-6b4592aa6b97"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Look"",
                    ""type"": ""Value"",
                    ""id"": ""938c6458-82e0-415a-8558-80e13a5c2a49"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""42e31f2c-0a0a-4359-8561-3957dae8f5a7"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Sprint"",
                    ""type"": ""PassThrough"",
                    ""id"": ""334ed996-59c4-4c01-9ff2-eaa4f7c8b5f0"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Slide"",
                    ""type"": ""Button"",
                    ""id"": ""1c464d02-4db2-4653-8192-8ef5b5b68074"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""FirstAbility"",
                    ""type"": ""Button"",
                    ""id"": ""191a3af2-ed98-4064-9442-220fb88d15ed"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""SecondAbility"",
                    ""type"": ""Button"",
                    ""id"": ""33394ebc-e2ec-4d2a-8cab-302a324c0f18"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""ThirdAbility"",
                    ""type"": ""Button"",
                    ""id"": ""1370ab41-4d5b-49bd-95cf-ad40930c8970"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""ESC"",
                    ""type"": ""Button"",
                    ""id"": ""3f14b984-82c8-4869-894e-0a5eed34f223"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""f1814afc-8905-49d4-b14c-221db90a32d0"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""30813629-2e69-42a0-8711-c0f1aff58e6d"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Sprint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""WASD"",
                    ""id"": ""7734d3aa-b72e-4381-ae02-6c4b3f1d6b7f"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""d7a45ecd-708c-4704-8097-debafa0f60cc"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""6ab63483-fd59-4a4c-aee9-7fe025f15b96"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""f8bf3c8f-2ddf-4745-a42c-c8bc2ecb5ed3"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""4c152521-bc49-40a5-a966-9e0ba958b0f8"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""8343ac90-85e1-4bd2-b250-16f6d81b2f21"",
                    ""path"": ""<Pointer>/delta"",
                    ""interactions"": """",
                    ""processors"": ""InvertVector2(invertX=false),ScaleVector2(x=5,y=5)"",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fed0fd75-ccea-4122-b4b7-20805b5fa34b"",
                    ""path"": ""<Keyboard>/leftCtrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Slide"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4299b469-dd6d-4780-8fdb-5b334271b18c"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""FirstAbility"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""047495a0-2a49-413b-a990-eba52489635e"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SecondAbility"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""37b0d039-c351-4c21-b050-c1411797d06f"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ThirdAbility"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a56f5ef5-d80f-4366-8d5d-b7ccf9cb8fc0"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ESC"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // GamePlay
        m_GamePlay = asset.FindActionMap("GamePlay", throwIfNotFound: true);
        m_GamePlay_Move = m_GamePlay.FindAction("Move", throwIfNotFound: true);
        m_GamePlay_Look = m_GamePlay.FindAction("Look", throwIfNotFound: true);
        m_GamePlay_Jump = m_GamePlay.FindAction("Jump", throwIfNotFound: true);
        m_GamePlay_Sprint = m_GamePlay.FindAction("Sprint", throwIfNotFound: true);
        m_GamePlay_Slide = m_GamePlay.FindAction("Slide", throwIfNotFound: true);
        m_GamePlay_FirstAbility = m_GamePlay.FindAction("FirstAbility", throwIfNotFound: true);
        m_GamePlay_SecondAbility = m_GamePlay.FindAction("SecondAbility", throwIfNotFound: true);
        m_GamePlay_ThirdAbility = m_GamePlay.FindAction("ThirdAbility", throwIfNotFound: true);
        m_GamePlay_ESC = m_GamePlay.FindAction("ESC", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // GamePlay
    private readonly InputActionMap m_GamePlay;
    private List<IGamePlayActions> m_GamePlayActionsCallbackInterfaces = new List<IGamePlayActions>();
    private readonly InputAction m_GamePlay_Move;
    private readonly InputAction m_GamePlay_Look;
    private readonly InputAction m_GamePlay_Jump;
    private readonly InputAction m_GamePlay_Sprint;
    private readonly InputAction m_GamePlay_Slide;
    private readonly InputAction m_GamePlay_FirstAbility;
    private readonly InputAction m_GamePlay_SecondAbility;
    private readonly InputAction m_GamePlay_ThirdAbility;
    private readonly InputAction m_GamePlay_ESC;
    public struct GamePlayActions
    {
        private @inp m_Wrapper;
        public GamePlayActions(@inp wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_GamePlay_Move;
        public InputAction @Look => m_Wrapper.m_GamePlay_Look;
        public InputAction @Jump => m_Wrapper.m_GamePlay_Jump;
        public InputAction @Sprint => m_Wrapper.m_GamePlay_Sprint;
        public InputAction @Slide => m_Wrapper.m_GamePlay_Slide;
        public InputAction @FirstAbility => m_Wrapper.m_GamePlay_FirstAbility;
        public InputAction @SecondAbility => m_Wrapper.m_GamePlay_SecondAbility;
        public InputAction @ThirdAbility => m_Wrapper.m_GamePlay_ThirdAbility;
        public InputAction @ESC => m_Wrapper.m_GamePlay_ESC;
        public InputActionMap Get() { return m_Wrapper.m_GamePlay; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GamePlayActions set) { return set.Get(); }
        public void AddCallbacks(IGamePlayActions instance)
        {
            if (instance == null || m_Wrapper.m_GamePlayActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_GamePlayActionsCallbackInterfaces.Add(instance);
            @Move.started += instance.OnMove;
            @Move.performed += instance.OnMove;
            @Move.canceled += instance.OnMove;
            @Look.started += instance.OnLook;
            @Look.performed += instance.OnLook;
            @Look.canceled += instance.OnLook;
            @Jump.started += instance.OnJump;
            @Jump.performed += instance.OnJump;
            @Jump.canceled += instance.OnJump;
            @Sprint.started += instance.OnSprint;
            @Sprint.performed += instance.OnSprint;
            @Sprint.canceled += instance.OnSprint;
            @Slide.started += instance.OnSlide;
            @Slide.performed += instance.OnSlide;
            @Slide.canceled += instance.OnSlide;
            @FirstAbility.started += instance.OnFirstAbility;
            @FirstAbility.performed += instance.OnFirstAbility;
            @FirstAbility.canceled += instance.OnFirstAbility;
            @SecondAbility.started += instance.OnSecondAbility;
            @SecondAbility.performed += instance.OnSecondAbility;
            @SecondAbility.canceled += instance.OnSecondAbility;
            @ThirdAbility.started += instance.OnThirdAbility;
            @ThirdAbility.performed += instance.OnThirdAbility;
            @ThirdAbility.canceled += instance.OnThirdAbility;
            @ESC.started += instance.OnESC;
            @ESC.performed += instance.OnESC;
            @ESC.canceled += instance.OnESC;
        }

        private void UnregisterCallbacks(IGamePlayActions instance)
        {
            @Move.started -= instance.OnMove;
            @Move.performed -= instance.OnMove;
            @Move.canceled -= instance.OnMove;
            @Look.started -= instance.OnLook;
            @Look.performed -= instance.OnLook;
            @Look.canceled -= instance.OnLook;
            @Jump.started -= instance.OnJump;
            @Jump.performed -= instance.OnJump;
            @Jump.canceled -= instance.OnJump;
            @Sprint.started -= instance.OnSprint;
            @Sprint.performed -= instance.OnSprint;
            @Sprint.canceled -= instance.OnSprint;
            @Slide.started -= instance.OnSlide;
            @Slide.performed -= instance.OnSlide;
            @Slide.canceled -= instance.OnSlide;
            @FirstAbility.started -= instance.OnFirstAbility;
            @FirstAbility.performed -= instance.OnFirstAbility;
            @FirstAbility.canceled -= instance.OnFirstAbility;
            @SecondAbility.started -= instance.OnSecondAbility;
            @SecondAbility.performed -= instance.OnSecondAbility;
            @SecondAbility.canceled -= instance.OnSecondAbility;
            @ThirdAbility.started -= instance.OnThirdAbility;
            @ThirdAbility.performed -= instance.OnThirdAbility;
            @ThirdAbility.canceled -= instance.OnThirdAbility;
            @ESC.started -= instance.OnESC;
            @ESC.performed -= instance.OnESC;
            @ESC.canceled -= instance.OnESC;
        }

        public void RemoveCallbacks(IGamePlayActions instance)
        {
            if (m_Wrapper.m_GamePlayActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IGamePlayActions instance)
        {
            foreach (var item in m_Wrapper.m_GamePlayActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_GamePlayActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public GamePlayActions @GamePlay => new GamePlayActions(this);
    public interface IGamePlayActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnLook(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnSprint(InputAction.CallbackContext context);
        void OnSlide(InputAction.CallbackContext context);
        void OnFirstAbility(InputAction.CallbackContext context);
        void OnSecondAbility(InputAction.CallbackContext context);
        void OnThirdAbility(InputAction.CallbackContext context);
        void OnESC(InputAction.CallbackContext context);
    }
}
