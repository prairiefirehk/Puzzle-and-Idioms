using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Ability
{
    #region Scripts
    public RoundData roundData;
    public UIManage uiManage;
    #endregion

    #region Game object references
    private Teammate _teammate;
    public Teammate teammate { get { return _teammate; } set { _teammate = value; } }
    #endregion

    #region Ability data
    private int _abilityID;
    public int abilityID { get { return _abilityID; } set { _abilityID = value; } }
    private string _abilityName;
    public string abilityName { get { return _abilityName; } set { _abilityName = value; } }
    private string _abiliityDesc;
    public string abilityDesc { get { return _abiliityDesc; } set { _abiliityDesc = value; } }
    private string _abilityType;
    public string abilityType { get { return _abilityType; } set { _abilityType = value; } }
    private string _abilityIconPicName;
    public string abilityIconPicName { get { return _abilityIconPicName; } set { _abilityIconPicName = value; } }

    private int _abilityLevel;
    public int abilityLevel { get { return _abilityLevel; } set { _abilityLevel = value; } }

    
    private int _baseAbilityCD;
    public int baseAbilityCD { get { return _baseAbilityCD; } set { _baseAbilityCD = value; } }
    private int _currentAbilityCD;
    public int currentAbilityCD { get { return _currentAbilityCD; } set { _currentAbilityCD = value; } }

    private int _baseAbilityCost;
    public int baseAbilityCost { get { return _baseAbilityCost; } set { _baseAbilityCost = value; } }
    private int _currentAbilityCost;
    public int currentAbilityCost { get { return _currentAbilityCost; } set { _currentAbilityCost = value; } }

    private List<int> _selfStatusEffectsID;
    public List<int>  selfStatusEffectsID { get { return _selfStatusEffectsID; } set { _selfStatusEffectsID = value; } }
    private List<int>  _selfStatusEffectsTurns;
    public List<int>  selfStatusEffectsTurns { get { return _selfStatusEffectsTurns; } set { _selfStatusEffectsTurns = value; } }
    private List<string> _selfAffectedStats;
    public List<string> selfAffectedStats { get { return _selfAffectedStats; } set { _selfAffectedStats = value; } }
    private List<int>  _selfAffectedStatsValue;
    public List<int>  selfAffectedStatsValue { get { return _selfAffectedStatsValue; } set { _selfAffectedStatsValue = value; } }
    private List<StatusEffect> _selfStatusEffects;
    public List<StatusEffect> selfStatusEffects { get { return _selfStatusEffects; } set { _selfStatusEffects = value; } }

    private List<int>  _enemyStatusEffectsID;
    public List<int>  enemyStatusEffectsID { get { return _enemyStatusEffectsID; } set { _enemyStatusEffectsID = value; } }
    private List<int>  _enemyStatusEffectsTurns;
    public List<int>  enemyStatusEffectsTurns { get { return _enemyStatusEffectsTurns; } set { _enemyStatusEffectsTurns = value; } }
    private List<string> _enemyAffectedStats;
    public List<string> enemyAffectedStats { get { return _enemyAffectedStats; } set { _enemyAffectedStats = value; } }
    private List<int>  _enemyAffectedStatsValue;
    public List<int>  enemyAffectedStatsValue { get { return _enemyAffectedStatsValue; } set { _enemyAffectedStatsValue = value; } }
    private List<StatusEffect> _enemyStatusEffects;
    public List<StatusEffect> enemyStatusEffects { get { return _enemyStatusEffects; } set { _enemyStatusEffects = value; } }

    // Entities
    private Entity _self;
    public Entity self { get { return _self; } set { _self = value; } }
    private Entity _enemy;
    public Entity enemy { get { return _enemy; } set { _enemy = value; } }
    
    #endregion

    #region Ability's status effect data
    //private List<>
    #endregion

    #region Ability functions
    // Initalizing the ability when the ability already registered inside mob/teammate
    public virtual void InitalizingAbility(Entity selfEntity, Entity enemyEntity)
    {
        Debug.Log($"{Time.time} {abilityName} Ability.InitalizingAbility (start)");

        this.self = selfEntity;
        this.enemy = enemyEntity;

        // Register the effect target, turns, level and adjust the number with level
        for (int i = 0; i < selfStatusEffects.Count; i++)
        {
            //Debug.Log($"$self.name = {self.name}");
            selfStatusEffects[i].effectTarget = self;
            //Debug.Log($"$selfStatusEffects[{i}].effectTarget.name = {selfStatusEffects[i].effectTarget.name}");
            selfStatusEffects[i].effectTurns = selfStatusEffectsTurns[i];
            //Debug.Log($"$selfStatusEffects[{i}] == null {selfStatusEffects[i] == null}");
            // Status effect level can be different from the ability, not nessarily related/equal
            selfStatusEffects[i].effectLevel = abilityLevel;
            //Debug.Log($"$selfStatusEffects[{i}].effectLevel = {selfStatusEffects[i].effectLevel}");

            for (int j = 0; j < selfStatusEffects[i].affectedStatsModifierValue.Count; j++)
            {
                selfStatusEffects[i].affectedStatsModifierValue[j] = selfStatusEffects[i].affectedStatsModifierValue[j] * selfStatusEffects[i].effectLevel;
            }   
        }
        
        for (int i = 0; i < enemyStatusEffects.Count; i++)
        {
            //Debug.Log($"$enemy.name = {enemy.name}");
            enemyStatusEffects[i].effectTarget = enemy;
            //Debug.Log($"$enemyStatusEffects[{i}].effectTarget.name = {enemyStatusEffects[i].effectTarget.name}");
            enemyStatusEffects[i].effectTurns = enemyStatusEffectsTurns[i];
            //Debug.Log($"$enemyStatusEffects[{i}] == null {enemyStatusEffects[i] == null}");
            // Status effect level can be different from the ability, not nessarily related/equal
            enemyStatusEffects[i].effectLevel = abilityLevel;
            //Debug.Log($"$enemyStatusEffects[{i}].effectLevel = {enemyStatusEffects[i].effectLevel}");

            for (int j = 0; j < enemyStatusEffects[i].affectedStatsModifierValue.Count; j++)
            {
                enemyStatusEffects[i].affectedStatsModifierValue[j] = enemyStatusEffects[i].affectedStatsModifierValue[j] * enemyStatusEffects[i].effectLevel;
            }   
        }

        currentAbilityCost = baseAbilityCost * abilityLevel;

        Debug.Log($"{Time.time} {abilityName} Ability.InitalizingAbility (end)");
    }

    public virtual void OnTrigger(Entity sourceEntity)
    {
        Debug.Log($"{Time.time} {abilityName} Ability.OnTrigger (start)");

        roundData = GameObject.Find("Round Manager").GetComponent<RoundData>();

        // Having not enough power score
        
        // Player spend the cost
        if (self == roundData.player && roundData.currentPowerScore >= currentAbilityCost)
        {
            roundData.currentPowerScore -= currentAbilityCost;
        }

        // Apply the status effect to self
        if (self != null) // Kinda on99
        {
            for (int i = 0; i < selfStatusEffects.Count; i++)
            {
                selfStatusEffects[i].OnInflict();
            }
        }

        // Apply the status effect to enemy
        if (enemy != null)
        {
            for (int i = 0; i < enemyStatusEffects.Count; i++)
            {
                enemyStatusEffects[i].OnInflict();
            }
        }

        // Apply the instant effect to self
        if (self != null) // Kinda on99
        {
            for (int i = 0; i < selfAffectedStats.Count; i++)
            {
                switch (selfAffectedStats[i])
                {
                    case "currentHealthValue":
                        self.currentHealthValue += selfAffectedStatsValue[i];
                        break;

                    case "currentActiveAbilityCD":
                        foreach (KeyValuePair<TeammateType, Teammate> teammate in self.GetComponent<Player>().teammates)
                        {
                            teammate.Value.currentActiveAbilityCD += selfAffectedStatsValue[i];
                        }
                        break;

                    case "currentAttackInterval":
                        self.GetComponent<Mob>().currentAttackInterval += selfAffectedStatsValue[i];
                        break;

                    case "powerScore":
                        roundData.currentPowerScore += selfAffectedStatsValue[i];
                        break;

                    case "currentTotalAttackValue":
                        foreach (KeyValuePair<TeammateType, Teammate> teammate in self.GetComponent<Player>().teammates)
                        {
                            teammate.Value.currentTotalAttackValue += selfAffectedStatsValue[i];
                        }
                        break;

                    // And more...
                }
            }
        }
        
        // Apply the instant effect to enemy
        if (enemy != null)
        {
            for (int i = 0; i < enemyAffectedStats.Count; i++)
            {
                switch (enemyAffectedStats[i])
                {
                    case "currentHealthValue":
                        enemy.currentHealthValue += enemyAffectedStatsValue[i];
                        break;

                    case "currentActiveAbilityCD":
                        foreach (KeyValuePair<TeammateType, Teammate> teammate in self.GetComponent<Player>().teammates)
                        {
                            teammate.Value.currentActiveAbilityCD += enemyAffectedStatsValue[i];
                        }
                        break;

                    case "currentAttackInterval":
                        self.GetComponent<Mob>().currentAttackInterval += enemyAffectedStatsValue[i];
                        break;

                    case "powerScore":
                        roundData.currentPowerScore += enemyAffectedStatsValue[i];
                        break;

                    case "currentTotalAttackValue":
                        foreach (KeyValuePair<TeammateType, Teammate> teammate in self.GetComponent<Player>().teammates)
                        {
                            teammate.Value.currentTotalAttackValue += enemyAffectedStatsValue[i];
                        }
                        break;

                    // And more...
                }
            }
        }
            
        // UI stuff
        uiManage = GameObject.Find("UI Manager").GetComponent<UIManage>();
        string popupMessage = $"{sourceEntity.entityName}發動咗技能{abilityDesc}!";
        uiManage.SpawnPopup(popupMessage, "special");

        Debug.Log($"{Time.time} {abilityName} Ability.OnTrigger (end)");
        
    }

    public virtual void UpdateAbilityTarget(Entity selfEntity, Entity enemyEntity)
    {
        Debug.Log($"{Time.time} {abilityName} Ability.UpdateAbilityTarget (start)");

        this.self = selfEntity;
        this.enemy = enemyEntity;

        // Update the effect target
        for (int i = 0; i < selfStatusEffects.Count; i++)
        {
            Debug.Log($"$self.name = {self.name}");
            selfStatusEffects[i].effectTarget = self;
            Debug.Log($"${selfStatusEffects[i].effectName} selfStatusEffects[{i}].effectTarget.name = {selfStatusEffects[i].effectTarget.name}");
        }
        
        for (int i = 0; i < enemyStatusEffects.Count; i++)
        {
            Debug.Log($"$enemy.name = {enemy.name}");
            enemyStatusEffects[i].effectTarget = enemy;
            Debug.Log($"${enemyStatusEffects[i].effectName} enemyStatusEffects[{i}].effectTarget.name = {enemyStatusEffects[i].effectTarget.name}");
        }

        Debug.Log($"{Time.time} {abilityName} Ability.UpdateAbilityTarget (end)");
    }
    #endregion

}

