#include "Component.h"
#include "Entity.h"

unsigned int Entity::s_uiEntityIDCount = 0;

std::map<const unsigned int, Entity*> Entity::s_xEntityList;

typedef std::pair<const unsigned int, Entity*> EntityPair;

Entity::Entity()
{
	//Increment entity ID
	m_uiEntityID = s_uiEntityIDCount++;

	//Add entity to list
	s_xEntityList.insert(EntityPair(m_uiEntityID, this));
}

Entity::~Entity()
{
}

void Entity::Update(float a_fDeltaTime)
{
	std::vector<Component*>::iterator xIter;
	for (xIter = m_apComponentList.begin(); xIter < m_apComponentList.end(); ++xIter)
	{
		Component* pComponent = *xIter;
		if (pComponent)
		{
			pComponent->Update(a_fDeltaTime);
		}
	}
}

void Entity::Draw(unsigned int a_uiProgramID, unsigned int a_uiVBO, unsigned int a_uiIBO)
{
	std::vector<Component*>::iterator xIter;
	for (xIter = m_apComponentList.begin(); xIter < m_apComponentList.end(); ++xIter)
	{
		Component* pComponent = *xIter;
		if (pComponent)
		{
			pComponent->Draw(a_uiProgramID, a_uiVBO, a_uiIBO);
		}
	}
}

Component* Entity::FindComponentOfType(COMPONENT_TYPE eComponentType)
{
	std::vector<Component*>::iterator xIter;
	for (xIter = m_apComponentList.begin(); xIter < m_apComponentList.end(); ++xIter)
	{
		Component* pComponent = *xIter;
		if (pComponent && pComponent->GetComponentType() == eComponentType)
		{
			return pComponent;
		}
	}
	return nullptr;
}