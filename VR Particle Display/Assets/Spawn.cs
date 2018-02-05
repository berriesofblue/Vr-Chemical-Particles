using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO; 
using UnityEngine;
using System.Text.RegularExpressions;

public class Spawn : MonoBehaviour {

	public GameObject particle;
    public string[] Structure;

	// Use this for initialization
	void Start () {
        Structure = File.ReadAllLines("Glycine.xyz");
        Match m;
        //while (!streamReader.EndOfStream)
        //{
        //	string[] lastPos = streamReader.ReadLine().Split(' ');
        //	GameObject Sphere = GameObject.Instantiate(particle);
        //	Sphere.transform.position = new Vector3(float.Parse(lastPos[0])*10, float.Parse(lastPos[1])*10, float.Parse(lastPos[2])*10);
        //}

        String pattern = @"^\s*(\S+?)\s+(\S+?)\s+(\S+?)\s+(\S+)";
            foreach (var expression in Structure)
            {
                Debug.Log(Regex.Match(expression, pattern).Value);
                if(Regex.Match(expression, pattern).Success)
                {
                    m = Regex.Match(expression, pattern);
                    GameObject Sphere = GameObject.Instantiate(particle);
                    Sphere.name = m.Groups[1].Value;
                    Sphere.GetComponent<Renderer>().material.color = GetCPK(m.Groups[1].Value);
                    Sphere.transform.position = new Vector3(float.Parse(m.Groups[2].Value), float.Parse(m.Groups[3].Value), float.Parse(m.Groups[4].Value));
                    MakeParticle(Sphere);
                }
            }
        
        MakeBonds();
    }

    void MakeParticle(GameObject ParticleObject)
    {
        ParticleObject.AddComponent<Particle>();
        Particle PInfo = ParticleObject.GetComponent<Particle>();
        PInfo.Notation = ParticleObject.name;
        AssignGroup(PInfo);
        PInfo.possiblebonds = 18 - PInfo.Group;
        PInfo.currentbonds = 0;
    }

    void AssignGroup(Particle particle)
    {
        switch (particle.Notation)
        {
            case "H":
            case "Li":
            case "Na":
            case "K":
            case "Rb":
            case "Cs":
            case "Fr":
                particle.Group = 1;
                break;
            case "Be":
            case "Mg":
            case "Ca":
            case "Sr":
            case "Ba":
            case "Ra":
                particle.Group = 2;
                break;
            case "Sc":
            case "Y":
            case "La":
            case "Ce":
            case "Pr":
            case "Nd":
            case "Pm":
            case "Sm":
            case "Eu":
            case "Gd":
            case "Tb":
            case "Dy":
            case "Ho":
            case "Er":
            case "Tm":
            case "Yb":
            case "Lu":
            case "Ac":
            case "Th":
            case "Pa":
            case "U":
            case "Np":
            case "Pu":
            case "Am":
            case "Cm":
            case "Bk":
            case "Cf":
            case "Es":
            case "Fm":
            case "Md":
            case "No":
            case "Lr":
                particle.Group = 3;
                break;
            case "Ti":
            case "Zr":
            case "Hf":
            case "Rf":
                particle.Group = 4;
                break;
            case "V":
            case "Nb":
            case "Ta":
            case "Db":
                particle.Group = 5;
                break;
            case "Cr":
            case "Mo":
            case "W":
            case "Sg":
                particle.Group = 6;
                break;
            case "Mn":
            case "Tc":
            case "Re":
            case "Bh":
                particle.Group = 7;
                break;
            case "Fe":
            case "Ru":
            case "Os":
            case "Hs":
                particle.Group = 8;
                break;
            case "Co":
            case "Rh":
            case "Ir":
            case "Mt":
                particle.Group = 9;
                break;
            case "Ni":
            case "Pd":
            case "Pt":
            case "Ds":
                particle.Group = 10;
                break;
            case "Cu":
            case "Ag":
            case "Au":
            case "Rg":
                particle.Group = 11;
                break;
            case "Zn":
            case "Cd":
            case "Hg":
            case "Cn":
                particle.Group = 12;
                break;
            case "B":
            case "Al":
            case "Ga":
            case "In":
            case "Tl":
            case "Nh":
                particle.Group = 13;
                break;
            case "C":
            case "Si":
            case "Ge":
            case "Sn":
            case "Pb":
            case "Fl":
                particle.Group = 14;
                break;
            case "N":
            case "P":
            case "As":
            case "Sb":
            case "Bi":
            case "Mc":
                particle.Group = 15;
                break;
            case "O":
            case "S":
            case "Se":
            case "Te":
            case "Po":
            case "Lv":
                particle.Group = 16;
                break;
            case "F":
            case "Cl":
            case "Br":
            case "I":
            case "At":
            case "Ts":
                particle.Group = 17;
                break;
            case "He":
            case "Ne":
            case "Ar":
            case "Kr":
            case "Xe":
            case "Rn":
            case "Og":
                particle.Group = 18;
                break;
            default:
                break;
        }
    }

    void MakeBonds()
    {
        RaycastHit hit;
        //find all particles
        GameObject[] Particles = GameObject.FindGameObjectsWithTag("Particle");
        Debug.Log(Particles.Length);
        //check how many bonds are missing
        for (int i = 0; i < Particles.Length; i++)
        {
            if(Particles[i].GetComponent<Particle>().currentbonds < Particles[i].GetComponent<Particle>().possiblebonds)
            {
                Dictionary<float, Transform> distances = new Dictionary<float, Transform>();

                for (int ia = 0; ia < Particles.Length; ia++)
                {
                    //if (Physics.Raycast(Particles[ia].transform.position, (Particles[i].transform.position - Particles[ia].transform.position), out hit, 5))
                    //{
                    Debug.DrawLine(Particles[ia].transform.position, Particles[i].transform.position, Color.white, 10f,true);
                    //}
                    distances.Add(Vector3.Distance(Particles[ia].transform.position, Particles[i].transform.position), Particles[ia].transform);
                }

                while(Particles[i].GetComponent<Particle>().possiblebonds > 0)
                {

                    var values = distances.Keys.ToList();
                    values.Sort();

                    int ic = 0;

                    while(Particles[i].GetComponent<Particle>().currentbonds != 18)
                    {
                        
                        Debug.DrawLine(distances[values[ic]].position, Particles[i].transform.position, Color.red, 10f, true);
                        Particles[i].GetComponent<Particle>().currentbonds++;
                        
                    }
                }



            }
        }
        //cast rays between all of them
        //find the closest 4 for each
        //check there are no overloaded ones
        //find new bonds accordingly
    }

    Color GetCPK(String Notation)
    {
        switch (Notation)
        {
            case "H":
                return Color.white;
            case "C":
                return Color.black;
            case "N":
                return Color.blue;
            case "O":
                return Color.red;
            case "F":
            case "Cl":
                return Color.green;
            case "Br":
                return new Color(155, 0, 0);
            case "I":
                return new Color(51, 0, 102);
            case "He":
            case "Ne":
            case "Ar":
            case "Xe":
            case "Kr":
                return Color.cyan;
            case "P":
                return new Color(255, 128, 0);
            case "S":
                return Color.yellow;
            case "B":
                return new Color(255, 204, 155);
            case "Li":
            case "Na":
            case "K":
            case "Rb":
            case "Cs":
            case "Fr":
                return new Color(102, 0, 204);
            case "Be":
            case "Mg":
            case "Ca":
            case "Sr":
            case "Ba":
            case "Ra":
                return new Color(0, 102, 0);
            case "Ti":
                return Color.gray;
            case "Fe":
                return new Color(153, 76, 0);
            default:
                return new Color(255, 0, 255);

        }
    }

	// Update is called once per frame
	void Update () {

	}
}

