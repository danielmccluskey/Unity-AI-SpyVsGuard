#include "Component.h"
#include "Entity.h"

#include <GL/glew.h>
#include <glm/ext.hpp>

Component::Component(Entity * a_pOwnerEntity)
	: m_pOwnerEntity(a_pOwnerEntity)
{
}

Component::~Component()
{
}

COMPONENT_TYPE Component::GetComponentType()
{
	return m_eComponentType;
}

Entity* Component::GetOwnerEntity()
{
	return m_pOwnerEntity;
}