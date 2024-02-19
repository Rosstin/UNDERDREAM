// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "MVVMViewModelBase.h"
#include "HeartlaceViewmodel.generated.h"

/**
 * 
 */
UCLASS()
class HEARTLACE_API UHeartlaceViewmodel : public UMVVMViewModelBase
{
	GENERATED_BODY()
	
	UPROPERTY(BlueprintReadWrite, FieldNotify, Setter, Getter)
	string HeartlaceOutputText;



};
