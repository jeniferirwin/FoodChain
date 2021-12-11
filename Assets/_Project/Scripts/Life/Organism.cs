using System;
using UnityEngine;
using FoodChain.Core;

namespace FoodChain.Life
{
    public abstract class Organism : MonoBehaviour, ICanBeEaten
    {
        public event Action<PercentPack> OnAgeTicked = delegate { };
        public event Action<int> OnAgeUp = delegate { };

        [SerializeField] protected OrganismTemplate organismTemplate;
        [SerializeField] protected GameObject infoPanel;

        // ENCAPSULATION
        // This thing has LAYERS
        public AgeData Age { get { return _ageData; } }
        public OrganismData Data { get { return _orgData; } }
        public float EnergyPercentValue { get { return Data.EnergyPercentValue; } }
        public int CurrentLifePhase { get { return Age.CurrentPhase; } }
        public PercentPack LifePhasePercent { get { return Age.PhasePercent; } }


        public GameObject Aggressor
        {
            get { return _aggressor; }
            set { if (Aggressor == null) _aggressor = value; }
        }

        protected AgeData _ageData;
        protected OrganismData _orgData;
        protected GameObject _aggressor;
        private MeshRenderer _rend;

        private bool InfoPanelVisibility
        {
            get
            {
                if (gameObject.CompareTag("Plant"))
                    return GameManager.PlantPanelsShowing;
                else if (gameObject.CompareTag("Herbivore"))
                    return GameManager.HerbivorePanelsShowing;
                else
                    return GameManager.CarnivorePanelsShowing;
            }
            set
            {
                if (gameObject.CompareTag("Plant"))
                    GameManager.PlantPanelsShowing = value;
                else if (gameObject.CompareTag("Herbivore"))
                    GameManager.HerbivorePanelsShowing = value;
                else
                    GameManager.CarnivorePanelsShowing = value;
            }
        }

        protected virtual void Awake()
        {
            // ABSTRACTION
            // SO MUCH ABSTRACTION
            OrganismTemplate _tmp = organismTemplate;
            _ageData = new AgeData(_tmp.phaseLengths, _tmp.phaseScales, _tmp.phaseMaterials);
            _orgData = new OrganismData(_tmp.energyPercentValue, _tmp.mainColorSlot);
            Aggressor = null;
            _rend = GetComponentInChildren<MeshRenderer>();
            SubscribeEvents();
            UpdateAppearance();
            OrganismDatabase.AddMember(gameObject);
        }

        protected virtual void Start()
        {
            infoPanel.SetActive(InfoPanelVisibility);
        }

        protected virtual void Update() => RunTickers();

        protected virtual void RunTickers() => Age.Tick();

        protected void UpdateAppearance()
        {
            if (Age.CurrentPhase >= 3) return;
            transform.localScale = Age.CurrentPhaseScale;
            var newMaterials = _rend.materials;
            newMaterials[Data.MainColorSlot] = Age.CurrentPhaseMaterial;
            _rend.materials = newMaterials;
        }

        protected virtual void SubscribeEvents()
        {
            Age.OnAgeUp += AgeUp;
            Age.OnAgeTicked += AgeTicked;
        }

        protected virtual void AgeUp(int newPhase)
        {
            if (newPhase == 3)
            {
                Cleanup();
                Die();
                return;
            }
            UpdateAppearance();
            OnAgeUp(newPhase);
        }

        protected virtual void AgeTicked(PercentPack pack) => OnAgeTicked(pack);

        protected virtual void Cleanup()
        {
            Age.Cleanup();
            OnAgeTicked = null;
            OnAgeUp = null;
        }

        public virtual void Die()
        {
            Cleanup();
            OrganismDatabase.RemoveMember(gameObject);
            Destroy(gameObject);
        }

        public virtual void ShowPanel(bool value)
        {
            InfoPanelVisibility = value;
            if (value)
                infoPanel.SetActive(true);
            else
                infoPanel.SetActive(false);
        }
    }
}
