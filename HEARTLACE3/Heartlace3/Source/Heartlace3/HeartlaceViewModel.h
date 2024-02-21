// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "MVVMViewModelBase.h"
#include "HeartlaceViewModel.generated.h"

/**
 * 
 */
UCLASS()
class HEARTLACE3_API UHeartlaceViewModel : public UMVVMViewModelBase
{
	GENERATED_BODY()

private:
	UPROPERTY(BlueprintReadWrite, FieldNotify, Setter, Getter, meta=(AllowPrivateAccess))
	FString OutputText;

	UPROPERTY(BlueprintReadWrite, FieldNotify, Setter, Getter, meta = (AllowPrivateAccess))
	int32 CurrentHealth;

	UPROPERTY(BlueprintReadWrite, FieldNotify, Setter, Getter, meta = (AllowPrivateAccess))
	int32 MaxHealth;

public:
	void SetOutputText(FString NewOutputText) {
		
		UE_MVVM_SET_PROPERTY_VALUE(OutputText, NewOutputText);
		UE_MVVM_BROADCAST_FIELD_VALUE_CHANGED(GetOutputText);
	}

	UFUNCTION(BlueprintPure, FieldNotify)
	FString GetOutputText() const {
		return OutputText;
	}

    void SetCurrentHealth(int32 NewCurrentHealth)
    {
        if (UE_MVVM_SET_PROPERTY_VALUE(CurrentHealth, NewCurrentHealth))
        {
            UE_MVVM_BROADCAST_FIELD_VALUE_CHANGED(GetHealthPercent);
        }
    }

    void SetMaxHealth(int32 newMaxHealth)
    {
        if (UE_MVVM_SET_PROPERTY_VALUE(MaxHealth, newMaxHealth))
        {
            UE_MVVM_BROADCAST_FIELD_VALUE_CHANGED(GetHealthPercent);
        }
    }

    int32 GetCurrentHealth() const
    {
        return CurrentHealth;
    }

    int32 GetMaxHealth() const
    {
        return MaxHealth;
    }

public:

    UFUNCTION(BlueprintPure, FieldNotify)

    float GetHealthPercent() const
    {
        if (MaxHealth != 0)
        {
            return (float)CurrentHealth / (float)MaxHealth;
        }
        else
            return 0;
    }

};

	
