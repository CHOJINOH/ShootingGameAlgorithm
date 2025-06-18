using UnityEngine;

public static class PatternBreeder
{
     public static BulletData Cross(BulletData a, BulletData b, float customMutationRate = 0.1f)
    {
        BulletData child = new BulletData();

        // 기본 유전자 교차
        child.bulletCount = ChooseGene(a.bulletCount, b.bulletCount);
        child.speed = ChooseGene(a.speed, b.speed);
        child.interval = ChooseGene(a.interval, b.interval);
        child.duration = ChooseGene(a.duration, b.duration);
        child.spreadAngle = ChooseGene(a.spreadAngle, b.spreadAngle);
        child.rotationPerShot = ChooseGene(a.rotationPerShot, b.rotationPerShot);
        child.type = Random.value < 0.5f ? a.type : b.type;

        // 탄막 형태 유전자
        child.shape = Random.value < 0.5f ? a.shape : b.shape;
        if (Random.value < customMutationRate)
            child.shape = (BulletShape)Random.Range(0, System.Enum.GetValues(typeof(BulletShape)).Length);

        // 형태별 파라미터
        if (child.shape == BulletShape.Spiral)
        {
            child.spiralSpeed = ChooseGene(a.spiralSpeed, b.spiralSpeed);
            if (Random.value < customMutationRate)
                child.spiralSpeed += Random.Range(-1f, 1f);
        }
        else if (child.shape == BulletShape.Wave)
        {
            child.waveAmplitude = ChooseGene(a.waveAmplitude, b.waveAmplitude);
            child.waveFrequency = ChooseGene(a.waveFrequency, b.waveFrequency);
            if (Random.value < customMutationRate)
                child.waveAmplitude += Random.Range(-0.5f, 0.5f);
            if (Random.value < customMutationRate)
                child.waveFrequency += Random.Range(-1f, 1f);
        }

        ApplyMutation(child, customMutationRate);
        return child;
    }



    // 부모 중 하나를 무작위로 선택
    private static int ChooseGene(int a, int b)
    {
        return Random.value < 0.5f ? a : b;
    }

    private static float ChooseGene(float a, float b)
    {
        return Random.value < 0.5f ? a : b;
    }

    // 변이: 확률적으로 약간 바꿈
    private static void ApplyMutation(BulletData data, float mutationRate)
    {
        if (Random.value < mutationRate)
            data.bulletCount += Random.Range(-1, 2);

        if (Random.value < mutationRate)
            data.speed += Random.Range(-2f, 2f);

        if (Random.value < mutationRate)
            data.interval += Random.Range(-0.05f, 0.05f);

        if (Random.value < mutationRate)
            data.spreadAngle += Random.Range(-20f, 20f);

        if (Random.value < mutationRate)
            data.rotationPerShot += Random.Range(-5f, 5f);

        if (Random.value < mutationRate)
            data.bulletCount += Random.Range(0, 2);

        if (Random.value < mutationRate)
            data.spreadAngle += Random.Range(10f, 30f);
    }

}
