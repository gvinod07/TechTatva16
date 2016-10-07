package com.purlieus.purlieus.adapters;

import android.content.Context;
import android.support.v4.app.FragmentActivity;
import android.support.v7.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.TextView;

import com.purlieus.purlieus.R;
import com.purlieus.purlieus.models.BD_Donate;
import com.purlieus.purlieus.models.BD_Seek;

import java.util.List;

/**
 * Created by Shaurya on 06-Oct-16.
 */

public class DonorAdapter extends RecyclerView.Adapter<DonorAdapter.UserViewHolder> {

private List<BD_Donate> list;
private LayoutInflater inflater;
private Context context;


public DonorAdapter(Context context, List<BD_Donate> list) {
        inflater = LayoutInflater.from(context);
        this.list = list;
        this.context = context;
        }

    @Override
public UserViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {

        View view = inflater.inflate(R.layout.item_donation, parent, false);

        UserViewHolder holder = new UserViewHolder(view);

        return holder;
        }

@Override
public void onBindViewHolder(UserViewHolder holder, int position) {

        BD_Donate model = list.get(position);
        holder.name.setText(model.getName());
        holder.bloodGroup.setText(model.getBloodGroup());
        holder.age.setText(Integer.toString(model.getAge()));
        holder.phoneNum.setText(model.getContactNumber());
        holder.sex.setText(model.getSex());

        }

@Override
public int getItemCount() {
        return list.size();
        }

class UserViewHolder extends RecyclerView.ViewHolder implements View.OnClickListener {

    TextView name, bloodGroup, phoneNum, sex, age;

    public UserViewHolder(View itemView) {
        super(itemView);

        itemView.setOnClickListener(this);
        name = (TextView) itemView.findViewById(R.id.donor_name_item);
        bloodGroup = (TextView) itemView.findViewById(R.id.donor_blood_group_item);
        phoneNum = (TextView) itemView.findViewById(R.id.donor_contact_item);
        sex = (TextView) itemView.findViewById(R.id.donor_sex_item);
        age = (TextView) itemView.findViewById(R.id.donor_age_item);

    }


    @Override
    public void onClick(View view) {

    }
}
}
