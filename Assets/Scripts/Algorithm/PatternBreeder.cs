using UnityEngine;

public static class PatternBreeder
{
    private static float mutationRate = 0.1f;

    public static BulletData Cross(BulletData a, BulletData b, float customMutationRate = 0.1f)
    {
        BulletData child = new BulletData();

        // 유전자 교차
        child.bulletCount = ChooseGene(a.bulletCount, b.bulletCount);
        child.speed = ChooseGene(a.speed, b.speed);
        child.interval = ChooseGene(a.interval, b.interval);
        child.duration = ChooseGene(a.duration, b.duration);
        child.spreadAngle = ChooseGene(a.spreadAngle, b.spreadAngle);
        child.rotationPerShot = ChooseGene(a.rotationPerShot, b.rotationPerShot);
        child.type = Random.value < 0.5f ? a.type : b.type;

        // 변이 적용
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
