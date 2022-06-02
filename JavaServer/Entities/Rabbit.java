package com.MyProject.lab2.Entities;

import com.fasterxml.jackson.annotation.JsonBackReference;

import javax.persistence.*;
import java.time.LocalDate;

@Entity
@Table(name = "rabbit")
public class Rabbit {

    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Id
    @Column(name = "id")
    private long id;
    public void setId(Long id) {
        this.id = id;
    }
    public Long getId() { return id; }

    @Column(name = "name")
    private String name;
    public void setName(String name) {
        this.name = name;
    }
    public String getName() {
        return name;
    }

    @Column(name = "color")
    private String color;
    public String getColor() { return color; }
    public void setColor(String color) { this.color = color; }

    @Column(name = "birth")
    private LocalDate birth = LocalDate.now();
    public LocalDate getBirth() { return birth; }

    @ManyToOne
    @JsonBackReference
    @JoinColumn(name = "owner_id", nullable = false)
    private Owner owner;
    public Owner getOwner() { return owner; }
    public void setOwner(Owner owner) { this.owner = owner; }

}
