#include "AudioComponent.h"
#include "Entity.h"
#include <glm/glm.hpp>

#include "TransformComponent.h"
#include <list>
typedef Component PARENT;

AudioComponent::AudioComponent(Entity * pOwner)
	: PARENT(pOwner)
{
	//Initialise the position of the sound to the center.
	Attributes3D.forward.z = 1.0f;
	Attributes3D.up.y = 1.0f;

	m_eComponentType = AUDIO;//Set the component type
}

AudioComponent::~AudioComponent()
{
}

void AudioComponent::Update(float a_fDeltaTime)
{
	Entity* pEntity = GetOwnerEntity();//Null check for the entity
	if (!pEntity)
	{
		return;
	}
	TransformComponent* pTransformComp = static_cast<TransformComponent*>(pEntity->FindComponentOfType(TRANSFORM));//Get the transform component
	if (pTransformComp)
	{
		UpdateEventPosition(pTransformComp->GetCurrentPosition());//Update the position of the events
	}
}

void AudioComponent::StartSound(int a_iSoundType)
{
	for (int i = 0; i < EventType.size(); i++)//Loop through event type array
	{
		if (EventType[i] == a_iSoundType)//If the event type is equal to the one given in the function
		{
			EventHolder[i]->start();//Start that sound
		}
	}
}

void AudioComponent::AddSound(FMOD::Studio::EventDescription*& a_pEventName, bool a_bStartInstantly, int a_iSoundType)
{
	FMOD::Studio::EventInstance* TempInstance;//Create a temp instance to copy
	a_pEventName->createInstance(&TempInstance);//Create an instance from the given Event description
	EventHolder.push_back(TempInstance);//Push that instance to the event holder array
	EventType.push_back(a_iSoundType);//Push the event type to the event type array

	if (a_bStartInstantly)
	{
		EventHolder[EventHolder.size() - 1]->start();//Start that event
	}
}

void AudioComponent::StopSound(int a_iSoundType)
{
	for (int i = 0; i < EventType.size(); i++)//Loop through event type array
	{
		if (EventType[i] == a_iSoundType)//If the event type is equal to the one given in the function
		{
			EventHolder[i]->stop(FMOD_STUDIO_STOP_IMMEDIATE);//Stop that sound
		}
	}
}

void AudioComponent::UpdateEventPosition(glm::vec3 a_v3Pos)
{
	//Clone the vector given in the arguments to the 3D Event attributes
	Attributes3D.position.x = a_v3Pos.x;
	Attributes3D.position.y = a_v3Pos.y;
	Attributes3D.position.z = a_v3Pos.z;

	for (int i = 0; i < EventHolder.size(); i++)//Loop through the event holder.
	{
		EventHolder[i]->set3DAttributes(&Attributes3D);//Set the events position.
	}
}