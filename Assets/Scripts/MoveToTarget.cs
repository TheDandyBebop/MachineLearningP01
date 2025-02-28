using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class MoveToTarget : Agent
{
[SerializeField] private Transform target;
    [SerializeField] private SpriteRenderer backgroundSpriteRenderer;
    public override void CollectObservations(VectorSensor sensor)
    {
        //Sensores de mi agente
        sensor.AddObservation((Vector2)transform.localPosition);
        sensor.AddObservation((Vector2)target.localPosition);
    }
    public override void OnActionReceived(ActionBuffers actions)
        //gestion movimiento
    {
        float moveX = actions.ContinuousActions[0];
        float moveY = actions.ContinuousActions[1];
        float movementSpeed = 5f;
        transform.localPosition += new Vector3(moveX, moveY) * Time.deltaTime *
        movementSpeed;
    }
    private void OnTriggerEnter2D(Collider2D collision)
        //Detecto contra que choco
    {
        if (collision.TryGetComponent(out Target target))
        {
            AddReward(10f);
            backgroundSpriteRenderer.color = Color.green;
            EndEpisode();
        }
        else if (collision.TryGetComponent(out Wall wall))
        {
            AddReward(-2f);
            backgroundSpriteRenderer.color = Color.red;
            EndEpisode();
        }
    }
    public override void OnEpisodeBegin() 
        //Reinicio la posicion inicial para que aprenda a llegar al objetivo
    {
        this.transform.localPosition = new Vector3(Random.Range(-3.5f, -1.5f),
        Random.Range(-3.5f, 3.5f));
        target.localPosition = new Vector3(Random.Range(1.5f, 3.5f),
        Random.Range(-3.5f, 3.5f));
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {//Permito la entrada del teclado para controlar al agente
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        continuousActions[1] = Input.GetAxisRaw("Vertical");
    }

}