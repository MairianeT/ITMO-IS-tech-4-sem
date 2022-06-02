package com.MyProject.lab2.Entities;

import javax.persistence.*;
import java.util.ArrayList;
import java.util.List;

@Entity
@Table(name = "owner")
public class Owner {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "id")
    private long id;
    public void setId(long id) {
        this.id = id;
    }
    public long getId() {
        return id;
    }

    @Column(name = "name")
    private String name;
    public String getName() {
        return name;
    }
    public void ListName(String name) {
        this.name = name;
    }

    @Column(name = "age")
    private int age;
    public int getAge() { return age; }
    public void setAge(int age) { this.age = age; }

    @OneToMany(mappedBy = "owner")
    private List<Rabbit> rabbits;
    public List<Rabbit> getRabbits() {
        return rabbits;
    }
    public void setRabbits(List<Rabbit> rabbits) {
        this.rabbits = rabbits;
    }
}
