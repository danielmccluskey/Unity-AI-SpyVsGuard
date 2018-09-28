#include "glm/ext.hpp"
#include "TransformComponent.h"
//typedefs
typedef Component PARENT;

TransformComponent::TransformComponent(Entity* pOwner)
	:PARENT(pOwner), m_m4EntityMatrix(glm::mat4())
{
	m_eComponentType = TRANSFORM;
}

TransformComponent::~TransformComponent()
{
}

glm::vec3 TransformComponent::GetRightDirection()
{
	return GetEntityMatrixRow(RIGHT_VECTOR);
}

glm::vec3 TransformComponent::GetUpDirection()
{
	return GetEntityMatrixRow(UP_VECTOR);
}

glm::vec3 TransformComponent::GetFacingDirection()
{
	return GetEntityMatrixRow(FORWARD_VECTOR);
}

glm::vec3 TransformComponent::GetCurrentPosition()
{
	return GetEntityMatrixRow(POSITION_VECTOR);
}

void TransformComponent::SetRightDirection(glm::vec3 vRightDirection)
{
	SetEntityMatrixRow(RIGHT_VECTOR, vRightDirection);
}

void TransformComponent::SetUpDirection(glm::vec3 vUpDirection)
{
	SetEntityMatrixRow(UP_VECTOR, vUpDirection);
}

void TransformComponent::SetFacingDirection(glm::vec3 vFacingDirection)
{
	SetEntityMatrixRow(FORWARD_VECTOR, vFacingDirection);
}

void TransformComponent::SetCurrentPosition(glm::vec3 vCurrentPosition)
{
	SetEntityMatrixRow(POSITION_VECTOR, vCurrentPosition);
}

/// <summary>
/// Gets the entity matrix.
/// </summary>
/// <returns>const glm.mat4 &.</returns>
const glm::mat4& TransformComponent::GetEntityMatrix()
{
	return m_m4EntityMatrix;
}

const void TransformComponent::SetEntityMatrix(glm::mat4 a_Matrix)
{
	m_m4EntityMatrix = a_Matrix;
}

void TransformComponent::SetEntityMatrixRow(MATRIX_ROW eRow, glm::vec3 vVec)
{
	m_m4EntityMatrix[eRow] = glm::vec4(vVec, (eRow == POSITION_VECTOR ? 1.f : 0.f));
}

glm::vec3 TransformComponent::GetEntityMatrixRow(MATRIX_ROW eRow)
{
	return m_m4EntityMatrix[eRow];
}